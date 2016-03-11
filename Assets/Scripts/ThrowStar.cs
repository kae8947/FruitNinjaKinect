using UnityEngine;
using System.Collections;
using System;

public class ThrowStar : MonoBehaviour {

	public Rigidbody starPrefab;
	public float Star_forward_Force = 1000;
	public float distanceToCheck = 100f;


	void Update () 
	{
		

			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			//GameObject Star = (GameObject)Instantiate (star_prefab, transform.position, Quaternion.LookRotation (pos));

			if(Input.GetButtonDown("Fire1"))
			{
				RaycastHit hit;
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				

				if(Physics.Raycast(ray, out hit, distanceToCheck)){
				
					Rigidbody starInstance;
					starInstance = Instantiate(starPrefab, transform.position, Quaternion.LookRotation (ray.direction)) as Rigidbody;
					starInstance.AddForce(transform.forward * Star_forward_Force);

				}
			}

	}


}
