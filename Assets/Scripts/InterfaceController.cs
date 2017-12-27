using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class InterfaceController : MonoBehaviour {

	public GameObject coin;
	public GameObject score;

	public GameObject increase10;
	public GameObject increase50;

	public GameObject HomeInterface;

	// Use this for initialization
	void Start () {
		coin.GetComponent<Text> ().text = "x " + 0;
		score.GetComponent<Text> ().text = "x " + 0;

		HomeInterface.SetActive (ApplicationModel.isHome);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public string updateUICoin(int newCoin)
	{
		return coin.GetComponent<Text>().text = "x " + newCoin;
	}

	public string updateUIScore(float newScore)
	{
		return score.GetComponent<Text>().text = "x " + (int)newScore;
	}
}
