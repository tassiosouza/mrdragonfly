using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gost : Enemy {

	private bool isGoingRight;
	private float velocity = 2f;
	private float velocityUp = 0;

	private Player player;

	private float velocityUpdate;
	float timetoanim = 0;

	protected bool startFade1 = false;
	protected int alpha1 = 100;

	// Use this for initialization
	void Start () {

		enemyID = ID_GOST;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
		player = FindObjectOfType<Player> ();
		animationController = GetComponent<Animator> ();
		killed = false;
		color = GetComponentInChildren<Renderer>().material.color;
	}

	// Update is called once per frame
	void FixedUpdate () {

		velocityUpdate = Time.deltaTime * velocity;

		if (gameController.IsGameRunning ()) {
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

			animationController.SetBool ("gameStarted", true);

//			if (player.gameObject.transform.position.y > this.transform.position.y - 1 &&
//				player.gameObject.transform.position.y < this.transform.position.y + 1) {
//
//				velocityUp = velocityUpdate / 2;
//				startFade1 = true;
//			}

			if (!startFade) {
				if (startFade1) {
					GetComponentInChildren<Renderer> ().material.color = new Color(color.r
						,color.g
						,color.b
						,alpha);


					alpha1 -= 1;
					if (alpha1 <= 50) {
						GetComponentInChildren<Renderer> ().enabled = !GetComponentInChildren<Renderer> ().enabled;

						if (alpha1 < 0) {
							Destroy (this.gameObject);
						}
					}
				}
			}


		}

		if (gameController.isGameEnded ()) {
			this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
			timetoanim += Time.deltaTime;

			if (timetoanim > 1) {
				animationController.SetBool ("gameStarted", false);

			}
		}
	}

	private void Move()
	{
		if (isGoingRight) {

			if (this.transform.position.x < 5) {
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate * LevelController.enemyVelocityFactor, 
					this.transform.position.y + velocityUp * LevelController.enemyVelocityFactor,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate * LevelController.enemyVelocityFactor,
					this.transform.position.y + velocityUp * LevelController.enemyVelocityFactor,
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
