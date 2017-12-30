using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pac : Enemy {

	private bool isGoingRight;
	private float velocity = 3f;

	private GameController gameController;
	Animator animationController;
	// Use this for initialization
	void Start () {

		enemyID = ID_PAC;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();

		animationController = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.IsGameRunning ()) {
			animationController.SetBool ("gameStarted", true);
			Move ();

			//destroy this when gets out of camera
			if (this.transform.position.y < this.gameController.mainCamera.transform.position.y - 15) {
				Destroy (this.gameObject);
			}
			animationController.SetBool ("kill", false);
		}

		if (gameController.isGameEnded ()) {
			animationController.SetBool ("gameStarted", false);
			this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
		}
	}

	private void Move()
	{
		float velocityUpdate = Time.deltaTime * velocity;

		if (isGoingRight) {

			if (this.transform.position.x < 5) {
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate, this.transform.position.y,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate, this.transform.position.y,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(225,Vector3.up);
			} else {
				isGoingRight = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			//GetComponent<Renderer>().material.color = new Color(255, 0, 0);
			Player player = other.GetComponent<Player>();
			animationController.SetBool ("kill", true);
			player.die ();
			this.gameController.endGame ();
		}
	}
}
