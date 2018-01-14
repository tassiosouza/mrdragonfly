using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static float gameVelocityFactor = 1;
	public static float enemyVelocityFactor = 1;
	public static float playerVelocityFactor = 0.75f;

	private float gameTime = 0;

	// Use this for initialization
	void Awake () {
		gameVelocityFactor = 1;
		enemyVelocityFactor = 1;

		gameTime = 0;
		
	}
	
	// Update is called once per frame
	void Update () {

		gameTime += Time.deltaTime;

		if (gameTime > 0 && gameTime <= 20)
		{
			playerVelocityFactor = 0.75f;
			gameVelocityFactor = 1.0f;
			enemyVelocityFactor = 1.2f;
		}
		if (gameTime > 20 && gameTime < 40)
		{
			enemyVelocityFactor = 1.4f;
		}
		if (gameTime > 40 && gameTime <= 60)
		{
			enemyVelocityFactor = 1.6f;
		}
		if (gameTime > 60 && gameTime < 80)
		{
			gameVelocityFactor = 1.2f;
			playerVelocityFactor = 0.68f;
			enemyVelocityFactor = 1.8f;
		}
		if (gameTime > 80 && gameTime <= 100)
		{
			enemyVelocityFactor = 2f;
		}
		if (gameTime > 100 && gameTime < 120)
		{
			enemyVelocityFactor = 2.2f;
		}
		if (gameTime > 120 && gameTime <= 140)
		{
			enemyVelocityFactor = 2.4f;
		}
		if (gameTime > 140 && gameTime < 160)
		{
			gameVelocityFactor = 1.4f;
			playerVelocityFactor = 0.55f;
			enemyVelocityFactor = 2.6f;
		}
		
	}
}
