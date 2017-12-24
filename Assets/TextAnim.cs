using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour {

	private float alpha = 1;
	private Text text;
	private float deltapositionY = 2f;

	private float coinPosX = 0;
	private float coinPosY = 0;

	Camera c;

	// Use this for initialization
	void Start () {
		text = this.GetComponent<Text>();
		c = Camera.main;
	}

	// Update is called once per frame
	void Update () {

		alpha -= 0.03f;
		deltapositionY += 4f;
		text.color = new Color(text.color.r,text.color.g,text.color.b,alpha);
		//text.transform.position = new Vector2 (375,707 + deltapositionY );
		float posx = c.WorldToScreenPoint(new Vector3(coinPosX,coinPosY + 1.5f,0)).x;
		float posy = c.WorldToScreenPoint(new Vector3(coinPosX,coinPosY + 1.5f,0)).y;
		Vector2 position = new Vector2(posx,posy);

		text.transform.position = new Vector2(position.x, position.y + deltapositionY);

		if (this.alpha < 0.05) {
			Destroy (this.gameObject);
		}
	}

	public void setInitialPosition(float x, float y)
	{
		coinPosX = x;
		coinPosY = y;
	}
}
