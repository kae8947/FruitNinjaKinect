using UnityEngine;
using System.Collections;

public class PathFinding2 : MonoBehaviour {

    LineRenderer path;
    public Transform startPos;
    public Transform endPos;
    private float textureOffset = 0f;
    private bool on = true;
    //private float pullSpeedFactor = 2f;
    private Vector3 endPosExtendedPos;
    private Vector3 diff;
    public bool find = true;
    public GameObject player;
    public GameObject target;
    void Start () {

        path = GetComponent<LineRenderer>();
        endPosExtendedPos = endPos.localPosition;

    }


    void Update () {

        find = true;




            /*  if (on)
              {
                  on = false;
              }
              else
              {
                  on = true;
              }
              */
        
        
   
  
        //if (find)
        //{
            
            player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            target.transform.position = GameObject.FindGameObjectWithTag("Target").transform.position;

        Vector3 startPos = player.transform.position;
        startPos.y -= .75f;
            path.enabled = true;
            path.SetPosition(0, startPos);
            path.SetPosition(1, target.transform.position);
           



        //}
        //else
        //{
        //    path.enabled = false;
        //}

        
        


    }

  


}
