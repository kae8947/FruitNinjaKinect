using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Kinect = Windows.Kinect;

public class TrackHand : MonoBehaviour
{
    public GameObject bodySourceManager;
    public GameObject objectToMove;

    private Dictionary<ulong, ulong> _Bodies = new Dictionary<ulong, ulong>();
    private BodySourceManager _BodyManager;

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

                MoveObjectWithJoint(body, Kinect.JointType.HandRight, Kinect.JointType.ShoulderRight);
            }
        }
    }

    float firstdeep = -1.0f;

    private void MoveObjectWithJoint(Kinect.Body body, Kinect.JointType hand, Kinect.JointType elbow)
    {
        Kinect.Joint? handJoint = null;
        Kinect.Joint? elbowJoint = null;

        handJoint = body.Joints[hand];
        elbowJoint = body.Joints[elbow];

        if (handJoint.HasValue && elbowJoint.HasValue)
        {
            //Vector3 handPosition = GetVector3FromJoint(targetJoint.Value, 20);
            //handPosition.z = objectToMove.transform.localPosition.z;
            //objectToMove.transform.localPosition = handPosition;

            if (body.HandRightState != Kinect.HandState.Closed)
            {
                //float horizontal = handJoint.Value.Position.X * 0.7f;
                //float vertical = handJoint.Value.Position.Y * 0.1f;
                //float deep = 0;

                float horizontal = (handJoint.Value.Position.X - elbowJoint.Value.Position.X) * 0.4f;
                float vertical = (handJoint.Value.Position.Y - elbowJoint.Value.Position.Y) * 0.4f;
                float deep = 0;

                if (firstdeep == -1)
                {
                    firstdeep = handJoint.Value.Position.Z * 0.05f;
                }

                deep = handJoint.Value.Position.Z * 0.05f - firstdeep;

                float newHorizontal = this.gameObject.transform.localPosition.x + horizontal;
                float newVertical = this.gameObject.transform.localPosition.y + vertical;

                //float newHorizontal = horizontal;
                //float newVertical = vertical;

                Vector3 newPos = new Vector3(
                    newHorizontal,
                    newVertical,
                    //this.transform.position.z - deep);
                    this.transform.localPosition.z);

                //newPos = bounds.ClosestPoint(newPos);

                this.gameObject.transform.localPosition = newPos;
            }
        }
    }

    private static Vector3 GetVector3FromJoint(Kinect.Joint joint, float scaleFactor)
    {
        return new Vector3(joint.Position.X * 10 * scaleFactor, joint.Position.Y * 10 * scaleFactor, joint.Position.Z * 10 * scaleFactor);
    }

    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
