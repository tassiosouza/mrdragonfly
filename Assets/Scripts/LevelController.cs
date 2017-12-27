using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour {

	public static float initialGameVelocity = 0;
	public static float deltaGameVelocity = 0;

	// Use this for initialization
	void Awake () {
		initialGameVelocity = Time.deltaTime * 2;
		deltaGameVelocity = Time.deltaTime/2000;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		initialGameVelocity = Time.deltaTime * 2;
		deltaGameVelocity = Time.deltaTime/2000;
		
	}
}
