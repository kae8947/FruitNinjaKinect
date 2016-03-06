using UnityEngine;
using System.Collections;

public class HealPlayer : MonoBehaviour {

	public float timeBetweenHeals = 0.5f;     
	public int healValue = 10; 
	                             
	GameObject player;                         
	PlayerHealth playerHealth;                                  
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

		if(timer >= timeBetweenHeals && playerInRange)
		{
			Heal ();
		}
	}


	void Heal ()
	{
		// Reset the timer.
		timer = 0f;

		playerHealth.TakeHeal (healValue);
	}
}
