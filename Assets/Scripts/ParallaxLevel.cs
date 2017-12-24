using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxLevel : MonoBehaviour {

	public int level = 0;
	private float acceleration = 0;

	public float currentRed = 1;
	public float currentGreen = 1;
	public float currentBlue = 1;

	public float time = 0;
	private float colorVelocity = 0.001f;

	public GameController gameController;
	SpriteRenderer rend;
	// Use this for initialization
	void Start () {
		rend = GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {

		if (gameController.IsGameRunning ()) {
			this.transform.position = new Vector3 (0,this.transform.parent.transform.position.y - (level *acceleration),
				this.transform.position.z);
			acceleration += 0.01f;

			if ((this.transform.localPosition.y) < -26.38) {
				this.transform.localPosition = new Vector3 (0,0,this.transform.localPosition.z);
				acceleration = 0;
			}

			colorControl ();

		}


	}

	private void colorControl()
	{
		time += colorVelocity;

		if (time > 0 && time < 0.5f) 
		{
			setColorUntilValue (0, 0.5f,false);
			setColorUntilValue (1, 0.5f,false);//azul escuro
			setColorUntilValue (2, 0.5f,false);
		}
	    if (time > 0.5f && time < 1) 
		{
			setColorUntilValue (1, 1f,true); // verde claro
			setColorUntilValue (0, 0f,false);
			setColorUntilValue (2, 0f,false);
		}
		if (time > 1f && time < 1.5f) 
		{
			setColorUntilValue (1, 0.5f,false);
			setColorUntilValue (0, 0.2f,false);// verde escuro
			setColorUntilValue (2, 0.2f,false);
		}
		if (time > 1.5f && time < 2.5f) 
		{
			setColorUntilValue (1, 0f,false);
			setColorUntilValue (0, 1f,true);// vermelho claro
			setColorUntilValue (2, 0f,false);
		}
		if (time > 2.5f && time < 3.5f) 
		{
			setColorUntilValue (1, 1f,true);
			setColorUntilValue (0, 1f,false);//azul escuro
			setColorUntilValue (2, 1,true);
		}
		if (time > 3.5f) 
		{
			time = 0;
		}


		rend.color = new Color(currentRed,currentGreen, currentBlue, 1f); // Set to opaque black
	}

	private void setColorUntilValue(int color, float value, bool isUp)
	{
		switch (color)
		{
		case 0://red

			if (!isUp) {
				if (currentRed >= value) {
					currentRed -= colorVelocity;
				}
			} else {
				if (currentRed <= value) {
					currentRed += colorVelocity;
				}
			}

			break;

			case 1://green
			
			if (!isUp) {
				if (currentGreen >= value) {
					currentGreen -= colorVelocity;
				}
			} else {
				if (currentGreen <= value) {
					currentGreen += colorVelocity;
				}
			}

			break;

			case 2://blue
			
			if (!isUp) {
				if (currentBlue >= value) {
					currentBlue -= colorVelocity;
				}
			} else {
				if (currentBlue <= value) {
					currentBlue += colorVelocity;
				}
			}


			break;
		}
	}
}
