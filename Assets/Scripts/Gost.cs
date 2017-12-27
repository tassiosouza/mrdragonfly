using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gost : Enemy {

	private bool isGoingRight;
	private float velocity = 4f;
	private float velocityUp = 0;

	private GameController gameController;
	private Player player;
	private bool startFade = false;
	private int alpha = 225;
	private Color color;

	private float velocityUpdate;
	Animator animationController;
	float timetoanim = 0;
	// Use this for initialization
	void Start () {

		enemyID = ID_GOST;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
		player = FindObjectOfType<Player> ();
		color = GetComponentInChildren<Renderer>().material.color;

		animationController = GetComponent<Animator> ();

	}

	// Update is called once per frame
	void FixedUpdate () {

		velocityUpdate = Time.deltaTime * velocity;

		if (gameController.IsGameRunning ()) {
			Move ();
			animationController.SetBool ("gameStarted", true);

			if (player.gameObject.transform.position.y > this.transform.position.y - 1 &&
				player.gameObject.transform.position.y < this.transform.position.y + 1) {

				velocityUp = velocityUpdate / 2;
				startFade = true;
			}

			if (startFade) {
				GetComponentInChildren<Renderer> ().material.color = new Color(color.r
																			 ,color.g
																			 ,color.b
																			 ,alpha);


				alpha -= 1;
				if (alpha <= 50) {
					GetComponentInChildren<Renderer> ().enabled = !GetComponentInChildren<Renderer> ().enabled;

					if (alpha < 0) {
						Destroy (this.gameObject);
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
				this.transform.position = new Vector3 (this.transform.position.x + velocityUpdate, this.transform.position.y + velocityUp,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -5) {
				this.transform.position = new Vector3 (this.transform.position.x - velocityUpdate, this.transform.position.y + velocityUp,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(225,Vector3.up);
			} else {
				isGoingRight = true;
			}
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			animationController.SetBool ("kill", true);
			Player player = other.GetComponent<Player>();
			player.die ();
			this.gameController.endGame ();
		}
	}
}
