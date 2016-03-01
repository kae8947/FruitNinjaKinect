using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerController : MonoBehaviour {

    public float speed;
    public Text countText;
    private Rigidbody rb;
    private int count;
    public Text winText;
	public Slider healthSlider;
	public PlayerHealth playerHealth;
	public int scoreValue = 10;
	public int healthValue = 50;
	GameObject Fruit;
    

	void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
        count = 0;
        //SetCountText();
        //winText.text = "";
		GetComponent<ScoreManager>();
    }

	void Update(){
		


	}


	void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
       
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
       
		rb.AddForce (movement * speed);
	}	




    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);

			playerHealth.currentHealth += healthValue;

			healthSlider.value = playerHealth.currentHealth;
            //count = count + 1;
            //SetCountText();
        }

		if (other.gameObject.CompareTag("Fruit")) {

			other.gameObject.GetComponent<ExplodingFruit>().Explode(); 	
			Destroy (other.gameObject, 3f);

			ScoreManager.score += scoreValue;
		}

    }

   /* void SetCountText ()
    {
        countText.text = "Count:" + count.ToString();
        if (count >= 12)
        {
            winText.text = "You Win!";
        }
    }*/
}
