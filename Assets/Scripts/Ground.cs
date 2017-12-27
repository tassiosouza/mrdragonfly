using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject babu;
	public GameObject pacCrazy;
	public GameObject gost;

	public GameObject coin;
	public GameObject cristal;
	public GameObject cristal1;

	private bool isMovingGround = false;

	// Use this for initialization
	void Start () {

		if (this.transform.position.y > 6) {
			if (Random.Range (0, 4) == 0) {
				Instantiate (pacCrazy);
				pacCrazy.GetComponent<Enemy>().setGround (this.gameObject);
				pacCrazy.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.position.y + 0.25f,
					this.transform.position.z);
			} else if(Random.Range (0, 4) == 2) {
				Instantiate (gost);
				gost.GetComponent<Enemy>().setGround (this.gameObject);
				gost.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.position.y + 0.25f,
					this.transform.position.z);
			}
			else {
				Instantiate (babu);
				babu.GetComponent<Enemy>().setGround (this.gameObject);
				babu.transform.position = new Vector3 (Random.Range (-5, 5), this.transform.position.y + 0.25f,
					this.transform.position.z);
			}

			//sort for moving ground
//			if(Random.Range (0, 4) == 0){
//				isMovingGround = true;
//				this.transform.GetChild(2).GetComponent<Renderer>().material.color = Color.red;
//			}
		}

		for (float i = - 3.5f; i <= 5.5f; i+=3.3f) {

			if (Random.Range (0, 6) == 1) {
				
				Instantiate (coin);
				coin.transform.position = new Vector3 (i, this.transform.position.y + 0.9f,
					this.transform.position.z);
			}
			else if(Random.Range (0, 4) == 2)
			{
				Instantiate (cristal);
				cristal.transform.position = new Vector3 (i, this.transform.position.y + 0.9f,
					this.transform.position.z);
			}
			else if(Random.Range (0, 10) == 3)
			{
				Instantiate (cristal1);
				cristal1.transform.position = new Vector3 (i, this.transform.position.y + 1.2f,
					this.transform.position.z);
			}
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
