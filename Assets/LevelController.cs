using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static float initialGameVelocity = 0;
	public static float deltaGameVelocity = 0;

	// Use this for initialization
	void Awake () {
		initialGameVelocity = Time.deltaTime * 5;
		deltaGameVelocity = Time.deltaTime/500;
		
	}
	
	// Update is called once per frame
	void Update () {

		initialGameVelocity = Time.deltaTime * 5;
		deltaGameVelocity = Time.deltaTime/500;
		
	}
}
