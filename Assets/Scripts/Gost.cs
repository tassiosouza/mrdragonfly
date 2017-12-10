using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gost : Enemy {

	private bool isGoingRight;
	private float velocity = 0.11f;
	private float velocityUp = 0;

	private GameController gameController;
	private Player player;
	private bool startFade = false;
	private int alpha = 225;
	private Color color;

	// Use this for initialization
	void Start () {

		enemyID = ID_GOST;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
		player = FindObjectOfType<Player> ();
		color = GetComponentInChildren<Renderer>().material.color;

	}

	// Update is called once per frame
	void Update () {
		if (gameController.IsGameRunning ()) {
			Move ();

			if (player.gameObject.transform.position.y > this.transform.position.y - 1 &&
				player.gameObject.transform.position.y < this.transform.position.y + 1) {

				velocityUp = velocity / 2;
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
	}

	private void Move()
	{
		if (isGoingRight) {

			if (this.transform.position.x < 7) {
				this.transform.position = new Vector3 (this.transform.position.x + velocity, this.transform.position.y + velocityUp,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -7) {
				this.transform.position = new Vector3 (this.transform.position.x - velocity, this.transform.position.y + velocityUp,
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
			other.gameObject.GetComponent<SphereCollider>().enabled = false;
			this.gameController.pauseGame ();
		}
	}
}
