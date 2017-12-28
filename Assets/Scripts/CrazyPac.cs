using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyPac : Enemy {

	private bool isGoingRight;
	private float velocity = 8;

	private GameController gameController;

	private float counter = 0;
	Animator animationController;
	// Use this for initialization
	void Start () {

		enemyID = ID_PAC;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();

		animationController = GetComponent<Animator> ();
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (gameController.IsGameRunning ()) {
			animationController.SetBool ("gameStarted", true);
			Move ();

			//destroy this when gets out of camera
			if (this.transform.position.y < this.gameController.mainCamera.transform.position.y - 15) {
				Destroy (this.gameObject);
			}
		}

		if (gameController.isGameEnded ()) {
			animationController.SetBool ("gameStarted", false);
		}
	}

	private void Move()
	{
		float velocityUpdate = Time.deltaTime * velocity;
		if (isGoingRight) {

			if (this.transform.position.x < 5) {
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate, this.transform.position.y,
					this.transform.position.z);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate, this.transform.position.y,
					this.transform.position.z);
			} else {
				isGoingRight = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			Player player = other.GetComponent<Player>();
			player.die ();
			animationController.SetBool ("kill", true);
			this.gameController.endGame ();
		}
	}
}
