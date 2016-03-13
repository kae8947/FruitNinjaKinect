using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class TrackHand : MonoBehaviour
{
    public GameObject bodySourceManager;
    public GameObject objectToMove;
    private Vector3 originalObjectToMovePos;

    private Dictionary<ulong, ulong> _Bodies = new Dictionary<ulong, ulong>();
    private BodySourceManager _BodyManager;

    void Start()
    {
        originalObjectToMovePos = Camera.main.WorldToScreenPoint(objectToMove.transform.position);
    }

    void Update()
    {
        //Debug.Log("target is " + Camera.main.WorldToScreenPoint(objectToMove.transform.position).x + " pixels from the left");

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

                MoveObjectWithJoint(body, Kinect.JointType.HandRight, Kinect.JointType.ShoulderRight);
            }
        }
    }

    private void MoveObjectWithJoint(Kinect.Body body, Kinect.JointType hand, Kinect.JointType elbow)
    {
        Kinect.Joint? handJoint = null;
        Kinect.Joint? elbowJoint = null;

        handJoint = body.Joints[hand];
        elbowJoint = body.Joints[elbow];

        if (handJoint.HasValue && elbowJoint.HasValue)
        {
            if (body.HandRightState != Kinect.HandState.Closed)
            {
                float horizontal = (handJoint.Value.Position.X - elbowJoint.Value.Position.X);
                float vertical = (handJoint.Value.Position.Y - elbowJoint.Value.Position.Y);

                Vector3 newPos = new Vector3(
                    transform.localPosition.x + horizontal,
                    transform.localPosition.y + vertical,
                    transform.localPosition.z);

                Vector3 colliderSize = GetComponent<BoxCollider>().size;

                Vector3 newPosTopLeft = new Vector3(newPos.x - (colliderSize.x / 2.0f), newPos.y + (colliderSize.y / 2.0f), newPos.z);
                Vector3 newPosBottomRight = new Vector3(newPos.x + (colliderSize.x / 2.0f), newPos.y - (colliderSize.y / 2.0f), newPos.z);

                Vector3 newTopLeftPosWorldPoint = transform.TransformPoint(newPosTopLeft);
                Vector3 newBottomRightPosWorldPoint = transform.TransformPoint(newPosBottomRight);

                Vector3 newTopLeftPosInScreenPoint = Camera.main.WorldToScreenPoint(newTopLeftPosWorldPoint);
                Vector3 newBottomRightPosInScreenPoint = Camera.main.WorldToScreenPoint(newBottomRightPosWorldPoint);

                Vector3 curPostion = transform.localPosition;

                // Check vertical bounds of object
                //if (newTopLeftPosInScreenPoint.y < Camera.main.pixelHeight && newBottomRightPosInScreenPoint.y > 0)
                //{
                //    curPostion.y = newPos.y;
                //}
                //else
                //{
                //    if (newTopLeftPosInScreenPoint.y > Camera.main.pixelHeight)
                //    {
                //        curPostion.y -= 5.0f;
                //    }
                //    else
                //    {
                //        curPostion.y += 5.0f;
                //    }
                //}

                if (newBottomRightPosInScreenPoint.x < Camera.main.pixelWidth && newTopLeftPosInScreenPoint.x > -0.1)
                {
                    curPostion.x = newPos.x;
                }
                else
                {
                    if (newBottomRightPosInScreenPoint.x > Camera.main.pixelWidth)
                    {
                        curPostion.x -= 1.0f;
                    }
                    else
                    {
                        curPostion.x += 1.0f;
                    }
                }

                transform.localPosition = curPostion;
            }
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint, float scaleFactor)
    {
        return new Vector3(joint.Position.X * 10 * scaleFactor, joint.Position.Y * 10 * scaleFactor, joint.Position.Z * 10 * scaleFactor);
    }
}
