using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalDialogController : MonoBehaviour {

	public GameObject topPart;
	public GameObject bottomPart;

	public GameController gameController;

	private float topX = -5625;//control the distance(time) to get to the screen
	private float bottomX = 5625;

	private float animVelocity = 2810f;

	private int currentScore = 0;
	private int bestScore = 0;

	public GameObject cScore;
	public GameObject bScore;

	// Use this for initialization
	void Start () {

		updateDialogPosition ();
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		updateDialogPosition ();

		if (gameController.isGameEnded()) {
			if(topX + Time.deltaTime * animVelocity <= 0)topX += Time.deltaTime * animVelocity;
			if(bottomX - Time.deltaTime * animVelocity >= 0)bottomX -= Time.deltaTime * animVelocity;
		}
			
	}

	private void updateDialogPosition()
	{
		topPart.transform.localPosition = new Vector3 (topX, topPart.transform.localPosition.y, topPart.transform.localPosition.z);
		bottomPart.transform.localPosition = new Vector3 (bottomX, bottomPart.transform.localPosition.y, bottomPart.transform.localPosition.z);
	}

	public void setInformations(int score, int best)
	{
		cScore.GetComponent<Text> ().text = ""+score;
		bScore.GetComponent<Text> ().text = ""+best;
	}
}
