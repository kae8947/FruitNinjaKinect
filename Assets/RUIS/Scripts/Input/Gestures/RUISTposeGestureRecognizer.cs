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
public class RUISTposeGestureRecognizer : RUISGestureRecognizer
{
    public int playerId = 0;
    public int bodyTrackingDeviceID = 0;

    public float maxMovementVelocity = 0.3f;
    public float timeBetweenRecognitions = 0.3f;
    public float handPositionDifferenceThreshold = 0.2f;
    public float requiredConfidence = 1.0f;
    //public PlayerHealing heal;

    public enum State
    {
        WaitingForTPose,
        MakingATPose,
        AfterTPose
    }
    public State currentState { get; private set; }


    private float timeCounter = 0;
    private bool gestureEnabled = true;


    public Vector3 leftHandPos { get; private set; }
    public Vector3 rightHandPos { get; private set; }
    public Vector3 leftShoulderPos { get; private set; }
    public Vector3 rightShoulderPos { get; private set; }
    public GameObject leftHandWithPointTracker;
    public GameObject rightHandWithPointTracker;

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
        pointTrackerRightHand = rightHandWithPointTracker.GetComponent<RUISPointTracker>();
        skeletonManager = FindObjectOfType(typeof(RUISSkeletonManager)) as RUISSkeletonManager;
        ResetProgress();
    }
    public void Start()
    {
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
        }
        else if (currentIsTracking != previousIsTracking)
        {
            StartCoroutine("StartCountdownTillGestureEnable");
        }

        previousIsTracking = currentIsTracking;

        if (!gestureEnabled || !isTrackingBufferTimeFinished) return;

        switch (currentState)
        {
            case State.WaitingForTPose:
                DoWaitingForTPose();
                break;
            case State.MakingATPose:
                DoMakingATPose();
                break;
            case State.AfterTPose:
                DoAfterTPose();
                break;
        }
    }

    public override bool GestureIsTriggered()
    {
        return gestureEnabled && currentState == State.MakingATPose;
    }

    public override bool GestureWasTriggered()
    {
        return false; // Not implemented
    }

    public override float GetGestureProgress()
    {
        return (gestureEnabled && currentState == State.MakingATPose) ? 1 : 0;
    }

    public override void ResetProgress()
    {
        currentState = State.WaitingForTPose;

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

    private void DoMakingATPose()
    {
        currentState = State.AfterTPose;
    }

    private void DoAfterTPose()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeBetweenRecognitions)
        {
            ResetProgress();
            return;
        }
    }

    private void DoWaitingForTPose()
    {
        if (skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.positionConfidence < requiredConfidence ||
            skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.positionConfidence < requiredConfidence)
        {
            return;
        }

        leftHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftHand.position;
        rightHandPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightHand.position;
        leftShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftShoulder.position;
        rightShoulderPos = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightShoulder.position;

        //print("LeftHand = " + leftHandPos + "\nRightHand = " + rightHandPos + "\nLeftShoulder = " + leftShoulderPos + "\nRightShoulder = " + rightShoulderPos);

        Vector3 leftHandDiffFromShoulder = leftShoulderPos - leftHandPos;
        Vector3 rightHandDiffFromShoulder = rightShoulderPos - rightHandPos;

        if (0.0 <= System.Math.Abs(leftHandDiffFromShoulder.y) && System.Math.Abs(leftHandDiffFromShoulder.y) <= handPositionDifferenceThreshold &&
            0.0 <= System.Math.Abs(leftHandDiffFromShoulder.z) && System.Math.Abs(leftHandDiffFromShoulder.z) <= handPositionDifferenceThreshold &&
            0.0 <= System.Math.Abs(rightHandDiffFromShoulder.y) && System.Math.Abs(rightHandDiffFromShoulder.y) <= handPositionDifferenceThreshold &&
            0.0 <= System.Math.Abs(rightHandDiffFromShoulder.z) && System.Math.Abs(rightHandDiffFromShoulder.z) <= handPositionDifferenceThreshold &&
            System.Math.Abs(pointTrackerLeftHand.averageVelocity.y) <= maxMovementVelocity &&
            System.Math.Abs(pointTrackerRightHand.averageVelocity.y) <= maxMovementVelocity)
        {
            //heal.healing();
            currentState = State.MakingATPose;
            timeCounter = 0;

            print("TPose Made");
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
