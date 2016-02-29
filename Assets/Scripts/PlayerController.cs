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
    
    

	void Start ()
	{
		rb = GetComponent<Rigidbody> ();
        count = 0;
        //SetCountText();
        winText.text = "";


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

			playerHealth.currentHealth += 50;

			healthSlider.value = playerHealth.currentHealth;
            //count = count + 1;
            //SetCountText();
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
