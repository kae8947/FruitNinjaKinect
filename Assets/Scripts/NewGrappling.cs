using UnityEngine;
using System;

public class NewGrappling : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100;
    public GameObject player;
    private Vector3 diff;
    private float pullSpeedFactor = 0.4f;
    float timer;
    Ray shootRay;
    bool attached = false;
    RaycastHit shootHit;
    int shootableMask;
    //ParticleSystem lightningParticle;
    LineRenderer grapple;
    //AudioSource gunAudio;
    float effectsDisplayTime = 0.2f;
    //private RUISBlastGestureRecognizer blastGesture;
    GameObject mytarget = null;

    void Awake ()
    {
        grapple = GetComponentInChildren<LineRenderer>();
        shootableMask = LayerMask.GetMask ("Fruit");
		//lightningParticle = GetComponentInChildren<ParticleSystem> ();
       // gunAudio = GetComponent<AudioSource> ();
      //  blastGesture = transform.parent.gameObject.GetComponentInChildren<RUISBlastGestureRecognizer>();

    }


    void Update ()
    {
       // timer += Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.Q)  /*|| BlastGestureTriggered() && timer >= timeBetweenBullets && Time.timeScale != 0*/)
        {
            Shoot ();
            if (attached) attached = false;
            else attached = true;

        }

        /*  if(timer >= timeBetweenBullets * effectsDisplayTime)
          {
              DisableEffects ();

          }*/

        if (attached && mytarget != null)
        {
            bool move = true;
            player.transform.position = GameObject.FindGameObjectWithTag("Player").transform.position;
            diff = mytarget.transform.position - player.transform.position;
            if (mytarget.layer == LayerMask.NameToLayer("Fruit"))
            {

                ExplodingFruit splash = mytarget.GetComponent<ExplodingFruit>();
                move = !splash.hasExploded;
            }
            if(Math.Abs(diff.z) < .02)
            {
                move = false;
                
            }
            if(move)
            {
              
                player.transform.position += diff / diff.magnitude * pullSpeedFactor;
            }
            else
            {
                grapple.enabled = false;
                
            }
        }
        else
        {
            grapple.enabled = false;
            
        }

    }


   

    void Shoot ()
    {
        //timer = 0f;

        //lightningParticle.Stop();

        GameObject nearestEnemey = findNearestTarget();
        shootRay.origin = transform.position;
        Vector3 distance = new Vector3(3, 3, 3);

        if (nearestEnemey != null)
        {
            grapple.enabled = true;
            grapple.SetPosition(0, transform.position);
            grapple.SetPosition(1, nearestEnemey.transform.position);

            distance = nearestEnemey.transform.position - transform.position;
            shootRay.direction = distance.normalized; // Normalized distance will give direction

            //var shape = lightningParticle.shape;
            //shape.length = distance.magnitude;

            Vector3 relativePos = nearestEnemey.transform.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(relativePos);
            //transform.parent.FindChild("FirstPersonCharacter").rotation = rotation;
            transform.parent.rotation = rotation;
        }
        else
        {
            shootRay.direction = transform.forward;
        }      

        if (Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
         
            //lightningParticle.Play();
            //gunAudio.Play();
        
           
            

            //EnemyHealth enemyHealth = shootHit.transform.gameObject.GetComponent<EnemyHealth>();
            //if(enemyHealth != null)
            //{
            //    enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            //}
        }
     

    }

    /* bool BlastGestureTriggered()
     {
         if (blastGesture == null || !blastGesture.GestureIsTriggered())
         {
             return false;
         }
         else
         {
             transform.position = blastGesture.GetTriggeredHand().transform.position;
             return true;
         }
     }*/

    GameObject findNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Shootable");

       
        float maxdistance = range;

        foreach (GameObject enemy in targets)
        {
            //work out which item is the closest and shoot at it, or find out which enemy has the most armour left or the most power and shoot it.

            float distance = Vector3.Distance(enemy.transform.position, transform.position);
            if (distance < maxdistance)
            {
                mytarget = enemy;
                maxdistance = distance;
            }
        }

        return mytarget;
    }

    void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("Shootable"))
        {
            grapple.enabled = false;
        }
    }

}
