using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

	float initialX;
	float initialY;

	Vector3 endPosition;

	private GameController gameController;

	void Awake()
	{
		gameController = FindObjectOfType<GameController> ();
	}

	// Use this for initialization
	void Start () {
		initialX = this.transform.position.x;
		initialY = this.transform.position.y;

		endPosition = new Vector3 (initialX - 20, initialY + 40, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Destroy (this.gameObject);
			gameController.increaseScore ();
		}
	}
}
