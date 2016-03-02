using UnityEngine;
using System.Collections;

public class Hook : MonoBehaviour {
    Animator anim;
    public float shootForceX = 0;
    public float shootForceY = 0;
    public float shootForceZ = 0;
    public GameObject prefabGrapple;
    public Transform Player;
    public Transform Target;

    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
       {

            anim.SetTrigger("Hook");
            //...spawn the grappling hook prefab
            GameObject InstanceGrapple = Instantiate(prefabGrapple, transform.position, Quaternion.LookRotation((Player.position - Target.position))) as GameObject;
                InstanceGrapple.GetComponent<Rigidbody>().AddForce(shootForceX, shootForceY, shootForceZ);
            //...shoot the spawned grappling hook with the forces set in the variables
            

        }
    }
}
