using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {
    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
       {

            anim.SetTrigger("Swing01");

        }
    }
}
