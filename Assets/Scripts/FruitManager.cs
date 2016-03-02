using UnityEngine;
using System.Collections;

public class FruitManager : MonoBehaviour {

	public PlayerHealth playerHealth;       
	public GameObject fruit;  
	public Transform player;
	public Transform spawnPoints;
	public GameObject rottenFruit;
	GameObject currentObject;

	void Start ()
	{
		// Call the Spawn function after a delay of the spawnTime and then continue to call after the same amount of time.

		InvokeRepeating ("Spawn", Random.Range (1.0F, 3.0F), Random.Range (1.0F, 3.0F));


	}

	void Update(){


		if (Input.GetKeyUp (KeyCode.C)) {
			if (currentObject != null)
				Destroy (currentObject);
			CancelInvoke ("switchObject");
			InvokeRepeating ("Spawn", Random.Range (1.0F, 3.0F), Random.Range (1.0F, 3.0F));
		}

		if (Input.GetKeyDown (KeyCode.C)) {

			if (currentObject != null)
				Destroy (currentObject);
			CancelInvoke ("Spawn");
			InvokeRepeating ("switchObject", Random.Range (1.0F, 3.0F), Random.Range (1.0F, 3.0F));
		}
	
	}


	void Spawn ()
	{
		// If the player has no health left...

		if (playerHealth.currentHealth <= 0f)
		{
			return;
		}

		currentObject = (GameObject)Instantiate(fruit, spawnPoints.position, Quaternion.LookRotation((player.position - spawnPoints.position)));
	}

	void switchObject(){

		if (playerHealth.currentHealth <= 0f)
		{
			return;
		}

		currentObject = (GameObject)Instantiate(rottenFruit, spawnPoints.position, Quaternion.LookRotation((player.position - spawnPoints.position)));

	}
}
