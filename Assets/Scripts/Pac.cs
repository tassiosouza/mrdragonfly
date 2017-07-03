using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pac : Enemy {

	private bool isGoingRight;
	private float velocity = 0.11f;

	private GameController gameController;

	// Use this for initialization
	void Start () {

		enemyID = ID_PAC;

		isGoingRight = (Random.Range (0, 1) == 1);
		gameController = FindObjectOfType<GameController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.IsGameRunning ()) {
			Move ();
		}
	}

	private void Move()
	{
		if (isGoingRight) {

			if (this.transform.position.x < 7) {
				this.transform.position = new Vector3 (this.transform.position.x + velocity, this.transform.position.y,
					this.transform.position.z);
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);
			} else {
				isGoingRight = false;
			}
		} else {
			if (this.transform.position.x > -7) {
				this.transform.position = new Vector3 (this.transform.position.x - velocity, this.transform.position.y,
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
