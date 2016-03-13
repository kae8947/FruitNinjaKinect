using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FruitManager : MonoBehaviour {

	public PlayerHealth playerHealth;       
	public GameObject fruit;  
	public Transform player;
	public Transform spawnPoints;
	public GameObject rottenFruit;
	GameObject currentObject;
	List<GameObject> myList = new List<GameObject>();
	string messageReceived;

	void Start ()
	{
		InvokeRepeating ("Spawn", Random.Range (1.0F, 3.0F), Random.Range (1.0F, 3.0F));
		messageReceived = GetComponent<UDPReceive> ().getLatestUDPPacket();
	}

	void Update(){

		if (playerHealth.currentHealth == playerHealth.startingHealth) {
			InvokeRepeating ("Spawn", Random.Range (1.0F, 3.0F), Random.Range (1.0F, 3.0F));
		}

		if (messageReceived == "Candy God Help Me") {
			CancelInvoke ("Spawn");
			switchObject();
		}
	}


	void Spawn ()
	{
		// If the player has no health left...

		/*if (playerHealth.currentHealth <= 0f)
		{
			return;
		}*/

		currentObject = (GameObject)Instantiate(fruit, spawnPoints.position, Quaternion.LookRotation((player.position - spawnPoints.position)));
		myList.Add (currentObject);
	}

	void switchObject(){
			
		List<GameObject> itemsToRemove = new List<GameObject>();
		foreach (GameObject obj in myList) {
			if (obj != null) 
			{
				Destroy(obj);
				itemsToRemove.Add(obj);
				Instantiate (rottenFruit, obj.transform.position, Quaternion.LookRotation ((player.position - obj.transform.position)));
			} 
			else 
			{
				itemsToRemove.Add(obj);
			}
		}

		foreach (GameObject obj in itemsToRemove) 
		{
			myList.Remove(obj);
		}
	}

}
