using UnityEngine;
using System.Collections;

public class SwingSword2 : MonoBehaviour
{
    Animator anim;
    public bool slicing = false;
    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {


            slicing = true;

        }

        if (slicing)
        {
            anim.SetTrigger("Swing02");
            StartCoroutine(Waiting());




        }



    }

    IEnumerator Waiting()
    {

        yield return new WaitForSeconds(0.7f);
        slicing = false;

    }


}

