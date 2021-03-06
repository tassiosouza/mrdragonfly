﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalDialogController : MonoBehaviour {

	public GameObject topPart;
	public GameObject bottomPart;

	public GameController gameController;

	private float topX = -3035;//control the distance(time) to get to the screen
	private float bottomX = 3035;

	private float animVelocity = 2810f;

	private int currentScore = 0;
	private int bestScore = 0;

	public GameObject cScore;
	public GameObject bScore;

	public GameDataController dataController;

	public GameObject medal;

	// Use this for initialization
	void Start () {

		updateDialogPosition ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		updateDialogPosition ();

		if (gameController.isGameEnded() && !gameController.isOnContinue()) {
			if(topX + Time.deltaTime * animVelocity <= 0)topX += Time.deltaTime * animVelocity;
			if(bottomX - Time.deltaTime * animVelocity >= 0)bottomX -= Time.deltaTime * animVelocity;
		}
			
	}

	private void updateDialogPosition()
	{
		topPart.transform.localPosition = new Vector3 (topX, topPart.transform.localPosition.y, topPart.transform.localPosition.z);
		bottomPart.transform.localPosition = new Vector3 (bottomX, bottomPart.transform.localPosition.y, bottomPart.transform.localPosition.z);
	}

	public void setInformations(int score)
	{
		//update best score if needed
		if (score > dataController.getBestScore ()) {
			dataController.setNewScore (score);
			medal.SetActive (true);
		}
		else 
		{
			medal.SetActive (false);
		}

		//set ui information
		cScore.GetComponent<Text> ().text = ""+ score;
		bScore.GetComponent<Text> ().text = ""+ dataController.getBestScore();

	}
}
