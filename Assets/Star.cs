using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour {

	private GameController gameController;

	void Awake()
	{
		gameController = FindObjectOfType<GameController> ();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * 50, Space.World);

		//destroy this when gets out of camera
		if (this.transform.position.y < this.gameController.mainCamera.transform.position.y - 15) {
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter(Collider other) {
		if (other.gameObject.tag == "Player") {
			
			Player player = other.GetComponent<Player>();
			player.setEspecial ();

			Destroy (this.gameObject);
		}
	}
}
