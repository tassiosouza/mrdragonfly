using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataController : MonoBehaviour {

	private string INSTALLED = "Installed";
	private int isInstalled = 0;

	private string SCORE = "Score";

	// Use this for initialization
	void Start () {
	
		try{
			isInstalled = PlayerPrefs.GetInt(INSTALLED);
		}
		catch(PlayerPrefsException) {
			isInstalled = 1;
			PlayerPrefs.SetInt (INSTALLED, isInstalled);

			//Init all game data
			PlayerPrefs.SetInt (SCORE, 0);
		}

	}

	public int getBestScore()
	{
		return PlayerPrefs.GetInt (SCORE);
	}

	public void setNewScore(int newScore)
	{
		PlayerPrefs.SetInt (SCORE, newScore);
	}
}
