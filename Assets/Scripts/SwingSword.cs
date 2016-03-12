using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {
    Animator anim;
    public bool hitting = false;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.F))
       {


           hitting = true;

        }

        if (hitting)
        {
            anim.SetTrigger("Swing01");
            StartCoroutine(Waiting());




        }



    }

    IEnumerator Waiting()
    {
        
        yield return new WaitForSeconds(0.7f);
        hitting = false;
        
    }


}
