using UnityEngine;
using System.Collections;

public class SwingSword : MonoBehaviour {
    public Animator anim;
    public bool hitting = false;
    private float atSword;
    private float atSwordTime = 2f;
    // Use this for initialization
    void Start () {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {

            anim.SetTrigger("Swing01");
            hitting = true;

        }

        if (hitting)
        {

            StartCoroutine(Waiting());




        }



    }

    IEnumerator Waiting()
    {

        yield return new WaitForSeconds(0.7f);
        hitting = false;

    }



    



    

 


}
