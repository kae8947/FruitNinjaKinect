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
    public bool inSpace;
	GameObject Fruit;
    public PlayerController boolBoy;
    Grappling grunt;
    LineRenderer oopa;
    SwingSword slash;
    SwingSword2 slice;


    void Awake ()
	{
		rb = GetComponent<Rigidbody> ();
        count = 0;
        //SetCountText();
        //winText.text = "";
		GetComponent<ScoreManager>();
        GameObject g = GameObject.FindGameObjectWithTag("Player").gameObject;
        grunt = GetComponentInChildren<Grappling>();
        slash = GetComponentInChildren<SwingSword>();
        slice = GetComponentInChildren<SwingSword2>();
        oopa = GetComponentInChildren<LineRenderer>();



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
            grunt.attached = false;
            oopa.enabled = false;
            other.gameObject.SetActive(false);

            if (playerHealth.currentHealth == playerHealth.startingHealth)
            {
                healthSlider.value = playerHealth.startingHealth;
            }
            else
            {
                playerHealth.currentHealth = playerHealth.currentHealth + healthValue;

                healthSlider.value = playerHealth.currentHealth;
            }
			
        }

		if (other.gameObject.CompareTag("Shootable")&& other.gameObject.layer == LayerMask.NameToLayer("Fruit") && grunt.attached == true) {

            grunt.attached = false;
            oopa.enabled = false;
			other.gameObject.GetComponent<ExplodingFruit>().Explode(); 	
			Destroy (other.gameObject, 3f);

			ScoreManager.score += scoreValue;
		}
        if (other.gameObject.CompareTag("Shootable") && other.gameObject.layer == LayerMask.NameToLayer("Fruit") && slash.hitting == true)
        {

           
            other.gameObject.GetComponent<ExplodingFruit>().Explode();
            Destroy(other.gameObject, 3f);
            slash.hitting = false;

            ScoreManager.score += scoreValue;
        }
        if (other.gameObject.CompareTag("Shootable") && other.gameObject.layer == LayerMask.NameToLayer("Fruit") && slice.slicing == true)
        {


            other.gameObject.GetComponent<ExplodingFruit>().Explode();
            Destroy(other.gameObject, 3f);
            slice.slicing = false;

            ScoreManager.score += scoreValue;
        }
    }
}
