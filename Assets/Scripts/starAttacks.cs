using UnityEngine;
using System.Collections;

public class starAttacks : MonoBehaviour {

	public int scoreValue = 10;

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Shootable")) {

			other.gameObject.GetComponent<ExplodingFruit>().Explode(); 	

			Destroy (other.gameObject, 3f);

			ScoreManager.score += scoreValue;
		}
	}
}
