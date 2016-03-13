using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class LeanTracking : MonoBehaviour
{
    public GameObject bodySourceManager;
    public float sideTollerance;
    public float forwardTollerance;
    public float turnSpeed = 50f;
    public float moveSpeed = 10f;

    private Dictionary<ulong, ulong> _Bodies = new Dictionary<ulong, ulong>();
    private BodySourceManager _BodyManager;

    private LeanType curLeanType = LeanType.Straight;

    public enum LeanType
    {
        Left,
        Right,
        Forward,
        Backward,
        Straight
    }

    void Update()
    {
        if (bodySourceManager == null)
        {
            return;
        }

        _BodyManager = bodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Kinect.Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        List<ulong> knownIds = new List<ulong>(_Bodies.Keys);

        // First delete untracked bodies
        foreach (ulong trackingId in knownIds)
        {
            if (!trackedIds.Contains(trackingId))
            {
                _Bodies.Remove(trackingId);
            }
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                if (!_Bodies.ContainsKey(body.TrackingId))
                {
                    _Bodies[body.TrackingId] = body.TrackingId;
                }

                curLeanType = calculateLean(body);

                Debug.Log(curLeanType.ToString());


            }
        }
    }

    public LeanType getLean()
    {
        return curLeanType;
    }

    int count = 0;

    private LeanType calculateLean(Kinect.Body body)
    {
        float shoulderLeft = getJointPosition(body, Kinect.JointType.ShoulderLeft).y;
        float shoulderRight = getJointPosition(body, Kinect.JointType.ShoulderRight).y;
        float head = getJointPosition(body, Kinect.JointType.Head).z;
        float spineMid = getJointPosition(body, Kinect.JointType.SpineMid).z;
        float shoulderDiff = System.Math.Abs(shoulderLeft - shoulderRight);
        float forwardDiff = System.Math.Abs(head - spineMid);

        if (count % 10 == 0)
        {
            Debug.Log("ShoulderDiff = " + shoulderDiff + "\n" + "ForwardDiff = " + forwardDiff);
        }

        if (shoulderDiff > sideTollerance)
        {
            if (shoulderLeft > shoulderRight)
            {
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
                // Leaning Right
                return LeanType.Right;
            }
            else
            {
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
                // Leaning Left
                return LeanType.Left;
            }
        }
        else
        {
            if (forwardDiff > forwardTollerance)
            {
                // Positive is away from Kinect
                if (head > spineMid)
                {
                    transform.Translate(-Vector3.forward * moveSpeed * Time.deltaTime);
                    // Leaning backward
                    return LeanType.Backward;
                }
                else
                {
                    transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
                    // Leaning forward
                    return LeanType.Forward;
                }
                
            }
            else
            {
                // No Direction
                return LeanType.Straight;
            }

        }
    }

    private Vector3 getJointPosition(Kinect.Body body, Kinect.JointType jt)
    {
        Kinect.Joint? targetJoint = null;

        targetJoint = body.Joints[jt];

        if (targetJoint.HasValue)
        {
            return GetVector3FromJoint(targetJoint.Value);
        }
        else
        {
            return new Vector3();
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
}
