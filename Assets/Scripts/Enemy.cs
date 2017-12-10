using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	public static int ID_PAC = 0;
	public static int ID_CRAZYPAC = 1;
	public static int ID_GOST = 2;

	protected int enemyID;
	protected GameObject ground;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setGround(GameObject ground)
	{
		this.ground = ground;
	}
}
