using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrazyPac : Enemy {

	private bool isGoingRight;
	private float velocity = 2f;

	private float counter = 0;

	// Use this for initialization
	void Start () {

		enemyID = ID_CRAZYPAC;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
		animationController = GetComponent<Animator> ();
		killed = false;
		color = GetComponentInChildren<Renderer>().material.color;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (gameController.IsGameRunning ()) {
			animationController.SetBool ("gameStarted", true);
			if (killed) {
				this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
				desappear ();
			} 
			else {
				Move ();
			}
			animationController.SetBool ("kill", false);
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
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate * LevelController.enemyVelocityFactor, this.transform.position.y,
					this.transform.position.z);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate * LevelController.enemyVelocityFactor, this.transform.position.y,
					this.transform.position.z);
			} else {
				isGoingRight = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		die (other);
	}
}
