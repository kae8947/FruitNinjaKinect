using UnityEngine;
using System.Collections;

public class SwingSword2 : MonoBehaviour {
    Animator anim;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
       {

            anim.SetTrigger("Swing02");

        }
    }
}
