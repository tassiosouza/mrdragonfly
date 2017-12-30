using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuePopupController : MonoBehaviour {

	private float initialPosX;
	private float velocity = 4;

	private bool coming = true;

	public GameController gameController;

	public GameObject timer;
	// Use this for initialization
	void Start () {

		reset ();
		
	}
	
	// Update is called once per frame
	void Update () {

		if (gameController.isGameEnded ()) {
			
			if (coming) {
				this.initialPosX -= Time.deltaTime * Mathf.Abs (initialPosX - 0) * velocity;



				if (this.initialPosX < 10) {
					timer.transform.localScale = new Vector3 (timer.transform.localScale.x - Time.deltaTime/3, 1, 1);

					if (Input.GetMouseButtonDown (0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) {
						coming = false;
						gameController.realEndGame ();	
					}



				}
			} else {
				this.initialPosX -= Time.deltaTime * Mathf.Abs (initialPosX - 50) * velocity * 2;
			}

			if (initialPosX < 0.5f) {

				if (timer.transform.localScale.x <= 0) {
					coming = false;
					gameController.realEndGame ();	
				}
				
			}
		}

		this.transform.localPosition = new Vector3 (initialPosX, this.transform.localPosition.y, 0);
	}

	public void reset(){
	
		this.initialPosX = 100650;
		this.transform.localPosition = new Vector3 (initialPosX, this.transform.localPosition.y, 0);
		timer.transform.localScale = new Vector3 (1, 1, 1);
	}
}
