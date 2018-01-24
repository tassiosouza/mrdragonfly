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
			acceleration += Time.deltaTime/4;

			if ((this.transform.localPosition.y) < -22.3) {
				this.transform.localPosition = new Vector3 (0,0,this.transform.localPosition.z);
				acceleration = 0;
			}

			colorControl ();

		}


	}

	private void colorControl()
	{
		time += Time.deltaTime;

		if (time > 0 && time < 15) 
		{
			setColorUntilValue (0, 0.5f,false);
			setColorUntilValue (1, 0.5f,false);//azul escuro
			setColorUntilValue (2, 0.5f,false);
		}
		if (time > 15 && time < 30) 
		{
			setColorUntilValue (1, 1f,true); // verde claro
			setColorUntilValue (0, 0f,false);
			setColorUntilValue (2, 0f,false);
		}
		if (time > 30f && time < 45f) 
		{
			setColorUntilValue (1, 0.5f,false);
			setColorUntilValue (0, 0.2f,false);// verde escuro
			setColorUntilValue (2, 0.2f,false);
		}
		if (time > 45f && time < 60f) 
		{
			setColorUntilValue (1, 0f,false);
			setColorUntilValue (0, 1f,true);// vermelho claro
			setColorUntilValue (2, 0f,false);
		}
		if (time > 60f && time < 75f) 
		{
			setColorUntilValue (1, 1f,true);
			setColorUntilValue (0, 1f,false);//azul escuro
			setColorUntilValue (2, 1,true);
		}
		if (time > 75f) 
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
					currentRed -= time/40000;
				}
			} else {
				if (currentRed <= value) {
					currentRed += time/40000;
				}
			}

			break;

			case 1://green
			
			if (!isUp) {
				if (currentGreen >= value) {
					currentGreen -= time/40000;
				}
			} else {
				if (currentGreen <= value) {
					currentGreen += time / 40000;
				}
			}

			break;

			case 2://blue
			
			if (!isUp) {
				if (currentBlue >= value) {
					currentBlue -= time/40000;
				}
			} else {
				if (currentBlue <= value) {
					currentBlue += time/40000;
				}
			}


			break;
		}
	}
}
