using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public static int ID_BABU = 1;
	public static int ID_CRAZYPAC = 2;
	public static int ID_GOST = 3;
	public static int ID_OGRE = 4;

	protected int enemyID;
	protected GameObject ground;

	protected bool startFade = false;
	protected int alpha = 100;
	protected Color color;

	protected Animator animationController;

	protected GameController gameController;
	protected bool killed = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	protected void desappear()
	{
		if (startFade) {
			GetComponentInChildren<Renderer> ().material.color = new Color (color.r
				, color.g
				, color.b
				, alpha);


			alpha -= 1;
			if (alpha <= 50) {
				GetComponentInChildren<Renderer> ().enabled = !GetComponentInChildren<Renderer> ().enabled;

				if (alpha < 0) {
					Destroy (this.gameObject);
				}
			}
		}
	}

	protected void die(Collider other)
	{
		if (other.gameObject.tag == "Player") {

			Player player = other.GetComponent<Player>();

			if (!player.isEspecial ()) {
				animationController.SetBool ("kill", true);
				player.die ();
				this.gameController.endGame ();
			} else
			{
				animationController.SetBool ("killed", true);
				killed = true;
				startFade = true;
			}
		}
	}

	public void setGround(GameObject ground)
	{
		this.ground = ground;
	}
}
