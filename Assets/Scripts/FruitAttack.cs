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

	void Awake ()
	{
		// Setting up the references.
		player = GameObject.FindGameObjectWithTag ("Player");
		playerHealth = player.GetComponent <PlayerHealth> ();
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
		if(timer >= timeBetweenAttacks && playerInRange)
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
		if(playerHealth.currentHealth > 0)
		{
			// ... damage the player.
			playerHealth.TakeDamage (attackDamage);
		}
	}
}
