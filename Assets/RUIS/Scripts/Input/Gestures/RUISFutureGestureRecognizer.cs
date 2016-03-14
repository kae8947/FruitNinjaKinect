/*****************************************************************************

Content    :   Implements a basic jump gesture
Authors    :   Mikael Matveinen
Copyright  :   Copyright 2013 Tuukka Takala, Mikael Matveinen. All Rights reserved.
Licensing  :   RUIS is distributed under the LGPL Version 3 license.

******************************************************************************/

using UnityEngine;
using System.Collections;
using CompleteProject;

[RequireComponent(typeof(RUISPointTracker))]
public class RUISFutureGestureRecognizer : RUISGestureRecognizer
{
    public int playerId = 0;
	public int bodyTrackingDeviceID = 0;

    public float timeBetweenRecognitions = 1.0f;
    public float handPositionDifferenceThreshold = 0.1f;
    public float requiredConfidence = 1.0f;
    //public CameraSwitch seeing;

    public enum State
    {
        WaitingForFuture,
        MakingFuture,
        AfterFuture
    }
    public State currentState { get; private set; }


    private float timeCounter = 0;
    private bool gestureEnabled = true;


    public Vector3 leftHandPos { get; private set; }
    public Vector3 rightHandPos { get; private set; }
    public Vector3 leftShoulderPos { get; private set; }
    public Vector3 rightShoulderPos { get; private set; }
    public Vector3 headPos { get; private set; }
    public GameObject leftHandWithPointTracker;
    public GameObject rightHandWithPointTracker;
    public GameObject headPointTracker;
    public bool isLeftHand;

    private RUISSkeletonManager skeletonManager;
    private RUISPointTracker pointTrackerLeftHand;
    private RUISPointTracker pointTrackerRightHand;
    private RUISPointTracker pointTrackerHead;
    private RUISSkeletonController skeletonController;

    private bool previousIsTracking = false;
    private bool isTrackingBufferTimeFinished = false;

    public void Awake()
    {
		skeletonController = FindObjectOfType(typeof(RUISSkeletonController)) as RUISSkeletonController;
        pointTrackerLeftHand = leftHandWithPointTracker.GetComponent<RUISPointTracker>();
        pointTrackerRightHand = rightHandWithPointTracker.GetComponent<RUISPointTracker>();
        pointTrackerHead = headPointTracker.GetComponent<RUISPointTracker>();
        skeletonManager = FindObjectOfType(typeof(RUISSkeletonManager)) as RUISSkeletonManager;
		ResetProgress();
    }
	public void Start() {
		
		bodyTrackingDeviceID = skeletonController.bodyTrackingDeviceID;
	}
    public void Update()
    {
        if (!skeletonManager) return;

		bool currentIsTracking = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].isTracking;

        if (!currentIsTracking)
        {
            previousIsTracking = false;
            isTrackingBufferTimeFinished = false;
            return;
        } else if (currentIsTracking != previousIsTracking)
        {
            StartCoroutine("StartCountdownTillGestureEnable");
        }

        previousIsTracking = currentIsTracking;

        if (!gestureEnabled || !isTrackingBufferTimeFinished) return;

        switch (currentState)
        {
            case State.WaitingForFuture:
                DoWaitingForFuture();
                break;
            case State.MakingFuture:
                DoMakingFuture();
                break;
            case State.AfterFuture:
                DoAfterFuture();
                break;
        }
    }

    public override bool GestureIsTriggered()
    {
        return gestureEnabled && currentState == State.MakingFuture;
    }
    
	public override bool GestureWasTriggered()
	{
		return false; // Not implemented
	}
	
    public override float GetGestureProgress()
    {
        return (gestureEnabled && currentState == State.MakingFuture) ? 1 : 0;
    }

    public override void ResetProgress()
    {
        currentState = State.WaitingForFuture;

        timeCounter = 0;
    }



    public override void EnableGesture()
    {
        gestureEnabled = true;
        ResetProgress();
    }

    public override void DisableGesture()
    {
        gestureEnabled = false;
    }

    private void DoMakingFuture()
    {
        currentState = State.AfterFuture;
    }

    private void DoAfterFuture()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeBetweenRecognitions)
        {
            ResetProgress();
            return;
        }
    }

    private void DoWaitingForFuture()
    {
		if (skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.positionConfidence < requiredConfidence ||
		    skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.positionConfidence < requiredConfidence ||
            skeletonManager.skeletons[bodyTrackingDeviceID, playerId].head.positionConfidence < requiredConfidence)
        {
            return;
        }

        if (isLeftHand)
        {
            leftHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.position;
            rightHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.position;
            leftShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftShoulder.position;
            rightShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightShoulder.position;
        }
        else
        {
            rightHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.position;
            leftHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.position;
            rightShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftShoulder.position;
            leftShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightShoulder.position;
        }
		
        headPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].head.position;

        //print("LeftHand = " + leftHandPos + "\nRightHand = " + rightHandPos + "\nLeftShoulder = " + leftShoulderPos + "\nRightShoulder = " + rightShoulderPos);

        Vector3 leftHandDiffFromShoulder = leftShoulderPos - leftHandPos;
        Vector3 rightHandDiffFromShoulder = rightShoulderPos - rightHandPos;
        Vector3 leftHandDiffFromHead = headPos - leftHandPos;


        if (0.0 <= System.Math.Abs(leftHandDiffFromHead.y) && System.Math.Abs(leftHandDiffFromHead.y) <= handPositionDifferenceThreshold &&
            0.0 <= System.Math.Abs(leftHandDiffFromHead.z) && System.Math.Abs(leftHandDiffFromHead.z) <= handPositionDifferenceThreshold &&
            0.0 <= System.Math.Abs(leftHandDiffFromHead.x) && System.Math.Abs(leftHandDiffFromHead.x) <= handPositionDifferenceThreshold
           )
        {
            currentState = State.MakingFuture;
            timeCounter = 0;

            print("Future Made");
            return;
        }

    }

    private IEnumerator StartCountdownTillGestureEnable()
    {
        yield return new WaitForSeconds(3.0f);

        isTrackingBufferTimeFinished = true;
    }
    
	public override bool IsBinaryGesture()
	{
		return true;
	}
}
