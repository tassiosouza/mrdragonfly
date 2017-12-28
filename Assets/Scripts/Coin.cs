using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	float initialX;
	float initialY;

	Vector3 endPosition;

	private GameController gameController;

	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	private float journeyLengthX;

	private Vector3 initialPosition;
	private Vector3 finalPosition;

	public GameObject increaseText;

	private bool hitted = false;

	void Awake()
	{
		gameController = FindObjectOfType<GameController> ();
	}

	// Use this for initialization
	void Start () {

		startTime = Time.time;
		initialPosition = this.transform.position;
		
		journeyLength = Vector3.Distance(initialPosition,finalPosition );
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * 20, Space.World);

		if (hitted) {
			journeyLength = Vector3.Distance(this.transform.position,finalPosition );

			if(this.gameObject.tag == "coin")
			journeyLengthX = 4.5f - this.transform.position.x;

			if(this.gameObject.tag == "cristal" || this.gameObject.tag == "cristal1" )
				journeyLengthX = -5f - this.transform.position.x;
			
			transform.position = new Vector3 (this.transform.position.x + journeyLengthX/15, this.transform.position.y + journeyLength/20,this.transform.position.z);

			if (journeyLength <= 3) {
				Destroy (this.gameObject);
			}
		}

		//destroy this when gets out of camera
		if (this.transform.position.y < this.gameController.mainCamera.transform.position.y - 15) {
			Destroy (this.gameObject);
		}

	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {

			if (this.gameObject.tag == "coin") {
				
				gameController.increaseCoin();
			}

			if (this.gameObject.tag == "cristal") {
				gameController.increaseScore (5);
			}

			if (this.gameObject.tag == "cristal1") {
				gameController.increaseScore (20);
			}
				
			//show text
			GameObject textToShow = Instantiate(increaseText) as GameObject;
			textToShow.GetComponent<TextAnim> ().setInitialPosition (this.transform.position.x, this.transform.position.y);
			textToShow.transform.SetParent(this.gameController.interfaceController.transform, false);


			//start animation to the top
			finalPosition = new Vector3 (this.transform.position.x, this.transform.position.y + (
				(this.gameController.mainCamera.transform.position.y + 16) - 
				this.transform.position.y),this.transform.position.z);
			hitted = true;
		}
	}
}
