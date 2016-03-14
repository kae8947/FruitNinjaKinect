/*****************************************************************************

Content    :   Implements a basic jump gesture
Authors    :   Mikael Matveinen
Copyright  :   Copyright 2013 Tuukka Takala, Mikael Matveinen. All Rights reserved.
Licensing  :   RUIS is distributed under the LGPL Version 3 license.

******************************************************************************/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RUISPointTracker))]
public class RUISBlastGestureRecognizer : RUISGestureRecognizer
{
    public int playerId = 0;
	public int bodyTrackingDeviceID = 0;

    public float maxMovementVelocity = 1.0f;
    public float timeBetweenRecognitions = 0.75f;
    public float handPositionMinDistanceInfront = 0.2f;
    public float handPositionSideToSideThreshold = 0.2f;
    public float handPositionUpDownThreshold = 0.2f;
    public float requiredConfidence = 1.0f;
    GameObject triggeredHand;
    public bool isLeftHand;

    public enum State
    {
        WaitingForBlast,
        MakingABlast,
        AfterBlast
    }
    public State currentState { get; private set; }


    private float timeCounter = 0;
    private bool gestureEnabled = true;


    public Vector3 leftHandPos { get; private set; }
    public Vector3 rightHandPos { get; private set; }
    public Vector3 leftShoulderPos { get; private set; }
    public Vector3 rightShoulderPos { get; private set; }
    public Vector3 spinePos { get; private set; }
    public GameObject leftHandWithPointTracker;

    private RUISSkeletonManager skeletonManager;
    private RUISPointTracker pointTrackerLeftHand;
    private RUISPointTracker pointTrackerRightHand;
    private RUISSkeletonController skeletonController;

    private bool previousIsTracking = false;
    private bool isTrackingBufferTimeFinished = false;

    public void Awake()
    {
		skeletonController = FindObjectOfType(typeof(RUISSkeletonController)) as RUISSkeletonController;
        pointTrackerLeftHand = leftHandWithPointTracker.GetComponent<RUISPointTracker>();
        skeletonManager = FindObjectOfType(typeof(RUISSkeletonManager)) as RUISSkeletonManager;
        triggeredHand = leftHandWithPointTracker;
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
            case State.WaitingForBlast:
                DoWaitingForBlast();
                break;
            case State.MakingABlast:
                DoMakingABlast();
                break;
            case State.AfterBlast:
                DoAfterJump();
                break;
        }
    }

    public override bool GestureIsTriggered()
    {
        return gestureEnabled && currentState == State.MakingABlast;
    }

    public GameObject GetTriggeredHand()
    {
        return triggeredHand;
    }
    
	public override bool GestureWasTriggered()
	{
		return false; // Not implemented
	}
	
    public override float GetGestureProgress()
    {
        return (gestureEnabled && currentState == State.MakingABlast) ? 1 : 0;
    }

    public override void ResetProgress()
    {
        currentState = State.WaitingForBlast;

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

    private void DoMakingABlast()
    {
        currentState = State.AfterBlast;
    }

    private void DoAfterJump()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeBetweenRecognitions)
        {
            ResetProgress();
            return;
        }
    }

    private void DoWaitingForBlast()
    {
		if (skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.positionConfidence < requiredConfidence ||
		    skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.positionConfidence < requiredConfidence)
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

        spinePos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].shoulderSpine.position;

        Vector3 leftHandDiffFromSpine = leftShoulderPos - leftHandPos;
        Vector3 rightHandDiffFromSpine = rightShoulderPos - rightHandPos;
        bool bastBeingMade = false;

        if (0.0 <= System.Math.Abs(leftHandDiffFromSpine.y) && System.Math.Abs(leftHandDiffFromSpine.y) <= handPositionUpDownThreshold &&
            0.0 <= System.Math.Abs(leftHandDiffFromSpine.x) && System.Math.Abs(leftHandDiffFromSpine.x) <= handPositionSideToSideThreshold &&
            System.Math.Abs(leftHandDiffFromSpine.z) >= handPositionMinDistanceInfront)
        {
            triggeredHand = leftHandWithPointTracker;
            bastBeingMade = true;
        }
        
        if (0.0 <= System.Math.Abs(leftHandDiffFromSpine.y) && System.Math.Abs(leftHandDiffFromSpine.y) <= handPositionUpDownThreshold &&
            0.0 <= System.Math.Abs(leftHandDiffFromSpine.x) && System.Math.Abs(leftHandDiffFromSpine.x) <= handPositionSideToSideThreshold &&
            System.Math.Abs(leftHandDiffFromSpine.z) >= handPositionMinDistanceInfront)
        {
            triggeredHand = leftHandWithPointTracker;
            bastBeingMade = true;
        }

        if (bastBeingMade && System.Math.Abs(pointTrackerLeftHand.averageVelocity.y) <= maxMovementVelocity)
        {
            currentState = State.MakingABlast;
            timeCounter = 0;

            print("Blast Made");
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
