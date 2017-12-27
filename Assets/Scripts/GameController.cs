using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float gameVelocity;
	public GameObject groundObject;

	public Camera mainCamera;

	private bool gameEnded = false;
	private bool gameStarted = false;

	private Button startButton;
	private Button friendsButton;
	private Button optionsButton;
	private RectTransform initialScreen;

	public Player player;

	public InterfaceController interfaceController;

	private float playerScore = 0;
	private int playerCoins = 0;

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

		gameVelocity = LevelController.initialGameVelocity;

		//comming from restart button
		if (!ApplicationModel.isHome)
		{
			startGame ();
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (IsGameRunning ()) {
			runGameScene ();
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

	public void increaseCoin()
	{
		playerCoins++;
		interfaceController.updateUICoin (playerCoins);
	}

	public void increaseScore(int value)
	{
		playerScore += value;
	}

	public void endGame()
	{
		this.interfaceController.GetComponentInChildren<FinalDialogController> ().setInformations ((int)playerScore,500);
		gameEnded = true;
	}

	public void startGame()
	{
		gameStarted = true;
		GameObject.Destroy (initialScreen.gameObject);
	}

	public void restartGame()
	{
		ApplicationModel.isHome = false;
		Application.LoadLevel ("Main");	
	}

	public void reloadGame()
	{
		ApplicationModel.isHome = true;
		Application.LoadLevel ("Main");	
	}

	public bool IsGameRunning()
	{
		return !gameEnded && gameStarted;
	}

	public bool isGameEnded()
	{
		return gameEnded;
	}

	private void runGameScene()
	{
		mainCamera.transform.position = new Vector3 (mainCamera.transform.position.x,
			mainCamera.transform.position.y + Time.deltaTime * 2 ,mainCamera.transform.position.z);
		
		//gameVelocity += LevelController.deltaGameVelocity;

		playerScore += Time.deltaTime;
		interfaceController.updateUIScore (playerScore);
	}

	public float getGameVelocity()
	{
		return this.gameVelocity;
	}

}
