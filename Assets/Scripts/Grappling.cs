using UnityEngine;
using System.Collections;

public class Grappling : MonoBehaviour {

    LineRenderer grappling;
    public Transform startPos;
    public Transform endPos;
    private float textureOffset = 0f;
    private bool on = true;
    private float pullSpeedFactor = 0.5f;
    private Vector3 endPosExtendedPos;
    private Vector3 diff;
    public bool attached;
    public GameObject player;
    public GameObject target;
    public GameObject objectSelect; 
    void Start () {

        
        grappling = GetComponent<LineRenderer>();
        endPosExtendedPos = endPos.localPosition;

    }


    void Update () {

        if(Input.GetKeyDown(KeyCode.Q))
        {

            objectSelect = GameObject.Find("cachedObj");
            if (objectSelect == null)
            {
                attached = false;
            }
            else
            {
                if (attached) attached = false;
                else attached = true;
            }



            /*  if (on)
              {
                  on = false;
              }
              else
              {
                  on = true;
              }
              */
        }
        
        if (on)
        {
            endPos.localPosition = Vector3.Lerp(endPos.localPosition, endPosExtendedPos, Time.deltaTime * 5f);
        }
        else
        {
            endPos.localPosition = Vector3.Lerp(endPos.localPosition, startPos.localPosition, Time.deltaTime * 5f);
        }
  
        if (attached)
        {
            
            player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            objectSelect.transform.position = GameObject.Find("cachedObj").transform.position;

            grappling.enabled = true;
            grappling.SetPosition(0, startPos.position);
            grappling.SetPosition(1, endPos.position);
            diff = objectSelect.transform.position - player.transform.position;
            player.transform.position += diff / diff.magnitude * pullSpeedFactor;



        }
        else
        {
            grappling.enabled = false;
        }

        
        


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Target")
        grappling.enabled = false;
        Destroy(target);
    }


}
