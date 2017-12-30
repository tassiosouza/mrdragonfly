using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

	public float gameVelocity;
	public GameObject groundObject;

	public Camera mainCamera;

	private bool gameEnded = false;
	private bool gameAlmostEnded = false;
	private bool gameStarted = false;

	private Button startButton;
	private Button friendsButton;
	private Button optionsButton;
	private RectTransform initialScreen;

	public Player player;

	public InterfaceController interfaceController;

	private float playerScore = 0;
	private int playerCoins = 0;

	private float timeToAddGrounds = 0;

	private List<Ground> groundList = new List<Ground>();

	public GameDataController gameDataController;

	public ContinuePopupController continuePopup;

	// Use this for initialization
	void Start () {

		addNewGrounds (20);

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

		playerCoins = gameDataController.getTotalCoins ();
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

	private void addNewGrounds(int quantity)
	{
		int size = groundList.Count + quantity;
		for (int i = groundList.Count; i < size; i++) {
			GameObject ground = Instantiate (groundObject);
			ground.transform.parent = this.transform;
			if (groundList.Count > 0) {
				ground.transform.position = new Vector3 (0, groundList [groundList.Count - 1].gameObject.transform.position.y + 2.7f, 0);
			} else {
				ground.transform.position = new Vector3 (0, 0, 0);
			}
			groundList.Add (ground.GetComponent<Ground>());
		}
	}

	public void increaseCoin()
	{
		gameDataController.incrementCoins (1);
		playerCoins = gameDataController.getTotalCoins ();
		interfaceController.updateUICoin (playerCoins);
	}

	public void decreaseCoin()
	{
		gameDataController.decrementCoins (10);
		playerCoins = gameDataController.getTotalCoins ();
		interfaceController.updateUICoin (playerCoins);
	}

	public void increaseScore(int value)
	{
		playerScore += value;
	}

	public bool isOnContinue()
	{
		return gameAlmostEnded;
	}

	public void realEndGame()
	{
		gameAlmostEnded = false;
	}


	public void endGame()
	{
		this.interfaceController.GetComponentInChildren<FinalDialogController> ().setInformations ((int)playerScore);
		gameEnded = true;
		gameAlmostEnded = true;
	}

	public void continueGame()
	{
		if (playerCoins >= 10) {

			decreaseCoin ();

			gameAlmostEnded = false;
			gameEnded = false;

			Player player = (Player)GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();
			Animator anim = (Animator)player.GetComponent<Animator> ();
			anim.SetBool ("continueGame", true);
			anim.SetBool ("isDeadJump", false);

			Ground middleGround = groundList [4];
			player.transform.position = new Vector3 (0, middleGround.transform.position.y, 0);

			player.GetComponent<SphereCollider> ().radius = player.GetComponent<SphereCollider> ().radius * 2f;

			foreach (GameObject enemy in GameObject.FindGameObjectsWithTag ("Enemy")) {
				if (enemy.transform.position.y < player.transform.position.y + 5 && enemy.transform.position.y > player.transform.position.y - 5) {
					Destroy (enemy);
				}
			}

			continuePopup.reset ();
		}
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

		//ground quantity controll : add
		timeToAddGrounds += Time.deltaTime;
		if (groundList[groundList.Count -1].transform.position.y < mainCamera.transform.position.y + 15) 
		{
			addNewGrounds (1);
		}
	}

	public List<Ground> getGroundList()
	{
		return this.groundList;
	}

	public float getGameVelocity()
	{
		return this.gameVelocity;
	}

}
