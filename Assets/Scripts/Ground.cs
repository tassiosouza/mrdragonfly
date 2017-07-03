using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour {

	public GameObject babu;
	public GameObject pacCrazy;

	public GameObject coin;

	// Use this for initialization
	void Start () {

		if (this.transform.position.y > 6) {
			if (Random.Range (0, 4) == 0) {
				Instantiate (pacCrazy);
				pacCrazy.transform.position = new Vector3 (Random.Range (-7, 7), this.transform.position.y + 0.25f,
					this.transform.position.z);
			} else {
				Instantiate (babu);
				babu.transform.position = new Vector3 (Random.Range (-7, 7), this.transform.position.y + 0.25f,
					this.transform.position.z);
			}
		}

		for (int i = - 6; i < 8; i+=2) {
			
			Instantiate (coin);
			coin.transform.position = new Vector3 (i, this.transform.position.y + 0.7f,
				this.transform.position.z);
		}


	}
	
	// Update is called once per frame
	void Update () {

		
	}
}
