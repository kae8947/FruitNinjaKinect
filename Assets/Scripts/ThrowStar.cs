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
		objectSeleted = GetComponentInChildren<GameObjectSelector> ().getSelectedObject ();

		if(Input.GetButtonDown("Fire1"))
		{
			if (objectSeleted != null) {
				Rigidbody starInstance;
				Vector3 starPosition = transform.position;
				starPosition.z += 5.0f;
				starInstance = Instantiate (starPrefab, starPosition, Quaternion.LookRotation (starPosition - objectSeleted.transform.position)) as Rigidbody;
				Vector3 shoot = (objectSeleted.transform.position - starPosition).normalized;
				starInstance.AddForce (shoot * Star_forward_Force);
			}


		}

	}


}
