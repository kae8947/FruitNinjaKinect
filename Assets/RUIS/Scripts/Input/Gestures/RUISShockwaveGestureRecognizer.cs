/*****************************************************************************

Content    :   Implements a basic jump gesture
Authors    :   Mikael Matveinen
Copyright  :   Copyright 2013 Tuukka Takala, Mikael Matveinen. All Rights reserved.
Licensing  :   RUIS is distributed under the LGPL Version 3 license.

******************************************************************************/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(RUISPointTracker))]
public class RUISShockwaveGestureRecognizer : RUISGestureRecognizer
{
    public int playerId = 0;
	public int bodyTrackingDeviceID = 0;

    public float requiredUpwardVelocity = 1.0f;
    public float timeBetweenJumps = 0.4f;
    public float feetHeightThreshold = 0.125f;
    public float requiredConfidence = 1.0f;
    //public Detonator other;

    public enum State
    {
        WaitingForShockwave,
        Shockwave,
        AfterShockwave
    }
    public State currentState { get; private set; }


    private float timeCounter = 0;
    private bool gestureEnabled = true;


    public Vector3 leftFootHeight { get; private set; }
    public Vector3 rightFootHeight { get; private set; }

    private RUISSkeletonManager skeletonManager;
    private RUISPointTracker pointTracker;
	private RUISSkeletonController skeletonController;

    private bool previousIsTracking = false;
    private bool isTrackingBufferTimeFinished = false;

    public void Awake()
    {
		skeletonController = FindObjectOfType(typeof(RUISSkeletonController)) as RUISSkeletonController;
        pointTracker = GetComponent<RUISPointTracker>();
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
            case State.WaitingForShockwave:
                DoWaitingForShockwave();
                break;
            case State.Shockwave:
                DoShockwave();
                break;
            case State.AfterShockwave:
                DoAfterShockwave();
                break;
        }
    }

    public override bool GestureIsTriggered()
    {
        return gestureEnabled && currentState == State.Shockwave;
    }
    
	public override bool GestureWasTriggered()
	{
		return false; // Not implemented
	}
	
    public override float GetGestureProgress()
    {
        return (gestureEnabled && currentState == State.Shockwave) ? 1 : 0;
    }

    public override void ResetProgress()
    {
        currentState = State.WaitingForShockwave;

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

    private void DoShockwave()
    {
        currentState = State.AfterShockwave;
    }

    private void DoAfterShockwave()
    {
        timeCounter += Time.deltaTime;

        if (timeCounter >= timeBetweenJumps)
        {
            ResetProgress();
            return;
        }
    }

    private void DoWaitingForShockwave()
    {
		if (skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftFoot.positionConfidence < requiredConfidence ||
		    skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightFoot.positionConfidence < requiredConfidence)
        {
            return;
        }

		leftFootHeight = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].leftFoot.position;
		rightFootHeight = skeletonManager.skeletons[bodyTrackingDeviceID, playerId].rightFoot.position;

        if (leftFootHeight.y >= feetHeightThreshold && rightFootHeight.y >= feetHeightThreshold && pointTracker.averageVelocity.y >= requiredUpwardVelocity)
        {
            //other.Explode();
            currentState = State.Shockwave;

            timeCounter = 0;
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
