﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject babu;
	public GameObject pacCrazy;
	public GameObject gost;
	public GameObject ogre;

	public GameObject coin;
	public GameObject cristal;
	public GameObject cristal1;

	private bool isMovingGround = false;

	// Use this for initialization
	void Start () {

		if (this.transform.position.y > 6) {
			if (Random.Range (0, 4) == 0) {
				GameObject pacCrazyObject = Instantiate (pacCrazy);
				pacCrazyObject.GetComponent<Enemy> ().setGround (this.gameObject);
				pacCrazyObject.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.localPosition.y + 2.7f,
					this.transform.position.z);

			} else if (Random.Range (0, 4) == 2) {
				GameObject gostObject = Instantiate (gost);
				gostObject.GetComponent<Enemy> ().setGround (this.gameObject);
				gostObject.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.localPosition.y + 2.7f,
					this.transform.position.z);

			} else if (Random.Range (0, 4) == 1) {
				GameObject ogreObject = Instantiate (ogre);
				ogreObject.GetComponent<Enemy>().setGround (this.gameObject);
				ogreObject.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.localPosition.y + 2.6f,
					this.transform.position.z);
				
			} else 
			{
				GameObject babuObject = Instantiate (babu);
				babuObject.GetComponent<Enemy> ().setGround (this.gameObject);
				babuObject.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.localPosition.y + 2.6f,
					this.transform.position.z);
				
			}


			//sort for moving ground
//			if(Random.Range (0, 4) == 0){
//				isMovingGround = true;
//				this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.red;
//			}

			for (float i = - 3.5f; i <= 5.5f; i+=3.3f) {

				if (Random.Range (0, 6) == 1) {

					GameObject coinObject = Instantiate (coin);
					coinObject.transform.position = new Vector3 (i, this.transform.position.y + 0.9f,
						this.transform.position.z);
					//coinObject.transform.parent = this.gameObject.transform;
				}
				else if(Random.Range (0, 4) == 2)
				{
					GameObject cristalObject = Instantiate (cristal);
					cristalObject.transform.position = new Vector3 (i, this.transform.position.y + 0.9f,
						this.transform.position.z);
					//cristalObject.transform.parent = this.gameObject.transform;
				}
				else if(Random.Range (0, 10) == 3)
				{
					GameObject cristal1Object = Instantiate (cristal1);
					cristal1Object.transform.position = new Vector3 (i, this.transform.position.y + 1.2f,
						this.transform.position.z);
					//cristal1Object.transform.parent = this.gameObject.transform;
				}
			}
		}




	}

	public void clearGround()
	{
		
	}

	public bool IsMovingGround()
	{
		return isMovingGround;
	}
	
	// Update is called once per frame
	void Update () {

		//destroy this when gets out of camera
		if (this.transform.position.y < Camera.main.transform.position.y - 15) {
			GameController gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();
			Destroy (this.gameObject);
			gameController.getGroundList ().Remove (this);
		}
		
	}
}
