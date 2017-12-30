using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataController : MonoBehaviour {

	private string INSTALLED = "Installed";
	private int isInstalled = 0;

	private string SCORE = "Score";
	private string COINS = "Coins";

	// Use this for initialization
	void Start () {
	
		try{
			isInstalled = PlayerPrefs.GetInt(INSTALLED);
		}
		catch(PlayerPrefsException) {
			isInstalled = 1;
			PlayerPrefs.SetInt (INSTALLED, isInstalled);

			resetData ();
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

	public int getTotalCoins()
	{
		return PlayerPrefs.GetInt (COINS);
	}

	public void incrementCoins(int quantityCoins)
	{
		PlayerPrefs.SetInt (COINS, getTotalCoins() + quantityCoins);
	}

	public void decrementCoins(int quantityCoins)
	{
		PlayerPrefs.SetInt (COINS, getTotalCoins() - quantityCoins);
	}

	public void resetData()
	{
		//Init all game data
		PlayerPrefs.SetInt (SCORE, 0);
		PlayerPrefs.SetInt (COINS, 0);
	}
}
