using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ogre : Enemy {

	private bool isGoingRight;
	private float velocity = 1f;

	private Player player;
	private float timeAnim = 0;

	// Use this for initialization
	void Start () {

		enemyID = ID_OGRE;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
		player = FindObjectOfType<Player> ();
		animationController = GetComponent<Animator> ();
		killed = false;
		color = GetComponentInChildren<Renderer>().material.color;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (gameController.IsGameRunning ()) {
			animationController.SetBool ("gameStarted", true);

			if (player.gameObject.transform.position.y > this.transform.position.y - 1 &&
			    player.gameObject.transform.position.y < this.transform.position.y + 1) {

				animationController.SetBool ("isOnLine", true);
				this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);

			} else {
				if (killed) {
					this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
					desappear ();
				} 
				else {
					Move ();
				}
			}
				
			if (animationController.GetBool ("isOnLine")) {
				timeAnim += Time.deltaTime;

				if (timeAnim > 1f) {
					if (killed) {
						this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
					} 
					else {
						Move ();
					}
					velocity = 1.5f;
				} else {
					isGoingRight = (player.transform.position.x >= this.transform.position.x);
				}

			}



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
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate * LevelController.enemyVelocityFactor, this.transform.position.y,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate * LevelController.enemyVelocityFactor, this.transform.position.y,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(225,Vector3.up);
			} else {
				isGoingRight = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		die (other);
	}
}
