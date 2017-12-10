using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float gameVelocity = 0.1f;
	public GameObject groundObject;

	public Camera mainCamera;

	private bool gamePaused = false;
	private bool gameStarted = false;

	private Button startButton;
	private Button friendsButton;
	private Button optionsButton;
	private RectTransform initialScreen;

	public Player player;

	public InterfaceController interfaceController;

	private int playerScore = 0;

	private List<Ground> groundList = new List<Ground>();

	// Use this for initialization
	void Start () {

		for (int i = 1; i < 100; i++) {
			GameObject ground = Instantiate (groundObject);
			ground.transform.parent = this.transform;
			ground.transform.position = new Vector3 (0, i * 2.7f, 0);
			groundList.Add (ground.GetComponent<Ground>());
		}

		QualitySettings.antiAliasing = 90000;

		startButton = GameObject.Find("Start Button").GetComponent<Button>();
		friendsButton = GameObject.Find("Friends Button").GetComponent<Button>();
		optionsButton = GameObject.Find("Options Button").GetComponent<Button>();
		initialScreen = GameObject.Find("Initial Screen").GetComponent<RectTransform>();
	}

	// Update is called once per frame
	void Update () {
		if (IsGameRunning ()) {
			runGameScene ();
		}
		else {
			if (Input.GetMouseButtonUp (0) && gameStarted) {
				Application.LoadLevel ("Main");			
			}
		}
	}

	public Ground getCurrentGround()
	{
		Ground currentGround = null;
		foreach(Ground ground in groundList)
		{
			if (ground.transform.position.y < player.gameObject.transform.position.y &&
			   ground.transform.position.y > player.gameObject.transform.position.y - 1) 
			{
				currentGround = ground;
			}
		}
		return currentGround;
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

	public void startGame()
	{
		gameStarted = true;
		GameObject.Destroy (initialScreen.gameObject);
	}


	public void restartGame()
	{
		gamePaused = false;
	}

	public bool IsGameRunning()
	{
		return !gamePaused && gameStarted;
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
