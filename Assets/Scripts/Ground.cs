using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject babu;
	public GameObject pacCrazy;
	public GameObject gost;

	public GameObject coin;

	private bool isMovingGround = false;

	// Use this for initialization
	void Start () {

		if (this.transform.position.y > 6) {
			if (Random.Range (0, 4) == 0) {
				Instantiate (pacCrazy);
				pacCrazy.GetComponent<Enemy>().setGround (this.gameObject);
				pacCrazy.transform.position = new Vector3 (Random.Range (-7, 7), this.transform.position.y + 0.25f,
					this.transform.position.z);
			} else if(Random.Range (0, 4) == 2) {
				Instantiate (gost);
				gost.GetComponent<Enemy>().setGround (this.gameObject);
				gost.transform.position = new Vector3 (Random.Range (-7, 7), this.transform.position.y + 0.25f,
					this.transform.position.z);
			}
			else {
				Instantiate (babu);
				babu.GetComponent<Enemy>().setGround (this.gameObject);
				babu.transform.position = new Vector3 (Random.Range (-7, 7), this.transform.position.y + 0.25f,
					this.transform.position.z);
			}

			//sort for moving ground
			if(Random.Range (0, 4) == 0){
				isMovingGround = true;
				this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.red;
			}
		}

		for (int i = - 6; i < 8; i+=2) {
			
			Instantiate (coin);
			coin.transform.position = new Vector3 (i, this.transform.position.y + 0.7f,
				this.transform.position.z);
		}


	}

	public bool IsMovingGround()
	{
		return isMovingGround;
	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
