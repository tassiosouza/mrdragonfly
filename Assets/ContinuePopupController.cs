using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
	bool showedAdd = false;
	// Update is called once per frame
	void Update () {

		if (gameController.isGameEnded ()) {
			
			if (coming) {
				this.initialPosX -= Time.deltaTime * Mathf.Abs (initialPosX - 0) * velocity;



				if (this.initialPosX < 10) {
					timer.transform.localScale = new Vector3 (timer.transform.localScale.x - Time.deltaTime/3, 1, 1);

					if (Input.GetMouseButtonDown (0) && !IsPointerOverUIObject()) {
						coming = false;
						gameController.realEndGame ();	
					}



				}
			} else {
				this.initialPosX -= Time.deltaTime * Mathf.Abs (initialPosX - 50) * velocity * 2;

				if (this.initialPosX < -300) {
					if(Random.Range(0,2) != 1)
					{
						showedAdd = true;
					}
					else
					{
						if (!showedAdd) {
							showedAdd = true;
							gameController.showAdd ();
						}
					} 

				}

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

	private bool IsPointerOverUIObject() {
		PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
		eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		List<RaycastResult> results = new List<RaycastResult>();
		EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
		return results.Count > 0;
	}
}
