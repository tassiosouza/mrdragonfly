using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundColorControl : MonoBehaviour {

	public float currentRed = 1;
	public float currentGreen = 1;
	public float currentBlue = 1;

	public float time = 0;
	private float colorVelocity = 0.001f;

	private GameController gameController;
	private Renderer rend;

	// Use this for initialization
	void Start () {
		rend = GetComponent<Renderer>();
		gameController = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameController>();


	}
	
	// Update is called once per frame
	void Update () {
		if (gameController.IsGameRunning ()) {
			
			//colorControl ();
			currentRed = GameObject.FindGameObjectWithTag ("ParallaxLevel").GetComponent<ParallaxLevel> ().currentRed;
			currentGreen = GameObject.FindGameObjectWithTag ("ParallaxLevel").GetComponent<ParallaxLevel> ().currentGreen;
			currentBlue = GameObject.FindGameObjectWithTag ("ParallaxLevel").GetComponent<ParallaxLevel> ().currentBlue;
			rend.material.color = new Color(currentRed,currentGreen, currentBlue, 1f); // Set to opaque black
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


		rend.material.color = new Color(currentRed,currentGreen, currentBlue, 1f); // Set to opaque black
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
