using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	public int attackDamage = 10;               
	                            
	GameObject fruit;                         
	//PlayerHealth playerHealth;                  
	EnemyHealth enemyHealth;                    
	bool playerInRange;                        
	float timer;                                

	void Awake ()
	{
		// Setting up the references.
		fruit = GameObject.FindGameObjectWithTag ("Shootable");
		//playerHealth = player.GetComponent <PlayerHealth> ();
		enemyHealth = GetComponent<EnemyHealth>();
		//anim = GetComponent <Animator> ();
	}


	void OnTriggerEnter (Collider other)
	{
		// If the entering collider is the player...
		if(other.gameObject == fruit)
		{
			// ... the player is in range.
			playerInRange = true;
		}
	}


	void OnTriggerExit (Collider other)
	{
		// If the exiting collider is the player...
		if(other.gameObject == fruit)
		{
			// ... the player is no longer in range.
			playerInRange = false;
		}
	}


	void Update ()
	{
		// Add the time since Update was last called to the timer.
		timer += Time.deltaTime;

		// If the timer exceeds the time between attacks, the player is in range and this enemy is alive...
		if(playerInRange)
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
		/*if(enemyHealth.currentHealth > 0)
		{
			// ... damage the fruit.
			enemyHealth.TakeDamage (attackDamage);
		}*/
	}

	void Split(){
	
	}
}
