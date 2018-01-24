using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static float gameVelocityFactor = 1;
	public static float enemyVelocityFactor = 1;
	public static float playerVelocityFactor = 1.5f;

	public static int gameLevel = 1;

	public static float gameTime = 0;

	GameController gamecontroller;

	// Use this for initialization
	void Awake () {
		gameVelocityFactor = 1;
		enemyVelocityFactor = 1;

		gameTime = 0;
		
	}

	void Start()
	{
		gamecontroller = (GameController)FindObjectOfType <GameController>();
	}
	
	// Update is called once per frame
	void Update () {

		if (gamecontroller.IsGameRunning () && !gamecontroller.isGameEnded()) {

			gameTime += Time.deltaTime;

			if (gameTime > 0 && gameTime <= 20) {
				//Player
				playerVelocityFactor = 1.07f;
				//Game
				gameVelocityFactor = 1.0f;
				//Enemy
				enemyVelocityFactor = 1.2f;
				gameLevel = 1;
			}
			if (gameTime > 20 && gameTime < 40) {
				enemyVelocityFactor = 1.4f;
				gameLevel = 2;
			}
			if (gameTime > 40 && gameTime <= 60) {
				enemyVelocityFactor = 1.6f;
				gameLevel = 3;
			}
			if (gameTime > 60 && gameTime < 80) {
				//Player
				playerVelocityFactor = 0.92f;
				//Game
				gameVelocityFactor = 1.15f;
				//Enemy
				enemyVelocityFactor = 1.8f;
				gameLevel = 4;
			}
			if (gameTime > 80 && gameTime <= 100) {
				enemyVelocityFactor = 2f;
				gameLevel = 5;
			}
			if (gameTime > 100 && gameTime < 120) {
				enemyVelocityFactor = 2.2f;
				gameLevel = 6;
			}
			if (gameTime > 120 && gameTime <= 140) {
				enemyVelocityFactor = 2.4f;
				gameLevel = 7;
			}
			if (gameTime > 140 && gameTime < 160) {
				//Player
				playerVelocityFactor = 0.81f;
				//Game
				gameVelocityFactor = 1.28f;
				//Enemy
				enemyVelocityFactor = 2.6f;
				gameLevel = 8;
			}
		}

	}

	public static void reset()
	{
		gameTime = 0;
		gameLevel = 1;
	}

	public static int getSortedEnemy()
	{
		int sortedNumber = Enemy.ID_BABU;

		switch (gameLevel) 
		{

			case 1: //############## Level 1 ###############

			sortedNumber = Random.Range (0, 5);

			if (sortedNumber > 0) {
				return Enemy.ID_BABU;
			} 
			else 
			{
				return sortedNumber;
			}
			
			break;

			case 2: //############## Level 2 ###############
			
			sortedNumber = Random.Range (0, 13);

			if (sortedNumber >= 1 && sortedNumber <= 5) {
				return Enemy.ID_BABU;
			} 
			else if (sortedNumber > 5) {
				return Enemy.ID_OGRE;
			} 
			else {
				return sortedNumber;
			}

			break;

			case 3: //############## Level 3 ###############

			sortedNumber = Random.Range (0, 18);

			if (sortedNumber >= 1 && sortedNumber <= 5) {
				return Enemy.ID_BABU;
			} 
			else if (sortedNumber >= 6 && sortedNumber <= 10) {
				return Enemy.ID_OGRE;
			} 
			else if (sortedNumber > 10) {
				return Enemy.ID_CRAZYPAC;
			} 
			else if (Random.Range(0,5) == 1){
				
				return sortedNumber;
			}

			break;

		    default: //############## Level 4 ###############

			sortedNumber = Random.Range (0, 23);

			if (sortedNumber >= 1 && sortedNumber <= 5) {
				return Enemy.ID_BABU;
			} 
			else if (sortedNumber >= 6 && sortedNumber <= 10) {
				return Enemy.ID_OGRE;
			} 
			else if (sortedNumber >= 11 && sortedNumber <= 15) {
				return Enemy.ID_CRAZYPAC;
			} 
			else if (sortedNumber > 15) {
				return Enemy.ID_GOST;
			} 
			else if (Random.Range(0,5) == 1){
				
				return sortedNumber;
			}

			break;
		}

		return sortedNumber;
	}
}
