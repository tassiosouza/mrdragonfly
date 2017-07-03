using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InterfaceController : MonoBehaviour {

	public GameObject score;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string updateUIScore(int newScore)
	{
		return score.GetComponent<Text>().text = "Score: " + newScore;
	}
}
