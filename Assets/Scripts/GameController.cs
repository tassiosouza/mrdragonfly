using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public float gameVelocity = 0.1f;
	public GameObject groundObject;

	public Camera mainCamera;

	private bool gamePaused = false;

	public InterfaceController interfaceController;

	private int playerScore = 0;

	// Use this for initialization
	void Start () {

		for (int i = 1; i < 100; i++) {
			GameObject ground = Instantiate (groundObject);
			ground.transform.parent = this.transform;
			ground.transform.position = new Vector3 (0, i * 2.7f, 0);
		}

		QualitySettings.antiAliasing = 100;
	}

	// Update is called once per frame
	void Update () {
		if (IsGameRunning ()) {
			runGameScene ();
		}
	}

	public void increaseScore()
	{
		playerScore++;
		interfaceController.updateUIScore (playerScore);
	}

	public void pauseGame()
	{
		gamePaused = true;
	}

	public void resumeGame()
	{
		gamePaused = false;
	}

	public void restartGame()
	{
		gamePaused = false;
	}

	public bool IsGameRunning()
	{
		return !gamePaused;
	}

	private void runGameScene()
	{
		mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x,
			mainCamera.transform.position.y + gameVelocity ,mainCamera.transform.position.z);
		
		gameVelocity += 0.0000005f;
	}

	public float getGameVelocity()
	{
		return this.gameVelocity;
	}

}
