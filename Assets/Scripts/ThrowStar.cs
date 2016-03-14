using UnityEngine;
using System.Collections;
using System;

public class ThrowStar : MonoBehaviour {

	public Rigidbody starPrefab;
	public float Star_forward_Force = 1000f;
	//public float distanceToCheck = 100f;
	GameObject objectSeleted;

	void Awake()
	{
		GetComponent<GameObjectSelector> ();
	}

	void Update () 
	{
		//Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		//GameObject Star = (GameObject)Instantiate (star_prefab, transform.position, Quaternion.LookRotation (pos));

		//objectSeleted = GetComponent<GameObjectSelector> ().getSelectedObject ();
		objectSeleted = GetComponentInChildren<GameObjectSelector> ().getSelectedObject ();

		if(Input.GetButtonDown("Fire1"))
		{
			//RaycastHit hit;
			//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (objectSeleted != null) {
				Rigidbody starInstance;
				Vector3 starPosition = transform.position;
				starPosition.z += 5.0f;
				starInstance = Instantiate (starPrefab, starPosition, Quaternion.LookRotation (starPosition - objectSeleted.transform.position)) as Rigidbody;
				starInstance.AddForce (transform.forward * Star_forward_Force);
			}


		}

	}


}
