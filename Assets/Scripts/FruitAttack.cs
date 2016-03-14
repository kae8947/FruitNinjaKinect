using UnityEngine;
using System.Collections;

public class FruitAttack : MonoBehaviour {

	public float timeBetweenAttacks = 0.5f;     
	public int attackDamage = 10;               

	//Animator anim;                              
	GameObject player;                         
	PlayerHealth playerHealth;                  
	//EnemyHealth enemyHealth;                    
	bool playerInRange;                        
	float timer;
    public SwingSword slash;
    public SwingSword2 slice;

    void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag ("Player");
        GameObject.Find("Player");

        playerHealth = player.GetComponent <PlayerHealth> ();
        slash = player.GetComponentInChildren<SwingSword>();
        
        slice = GetComponentInChildren<SwingSword2>();

    }


	void OnTriggerEnter (Collider other)
	{
		// If the entering collider is the player...
		if(other.gameObject == player)
		{
			// ... the player is in range.
			playerInRange = true;
		}
	}


	void OnTriggerExit (Collider other)
	{
		// If the exiting collider is the player...
		if(other.gameObject == player)
		{
			// ... the player is no longer in range.
			playerInRange = false;
			Destroy(gameObject);

		}
	}


	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

        // If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
        if (timer >= timeBetweenAttacks && playerInRange)
		{
			// ... attack.
			Attack ();
		}
	}


	void Attack ()
	{
		// Reset the timer.
		timer = 0f;

        // If the player has health to lose...
        if (playerHealth.currentHealth > 0)
		{
            // ... damage the player.
            if (slash.hitting == false)
            {
                playerHealth.TakeDamage(attackDamage);
            }
            else
            {
                Destroy(gameObject);
            }
		}
	}
}
