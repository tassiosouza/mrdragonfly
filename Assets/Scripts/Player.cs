using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public float jumpForce = 6.35f;
	private float lastYPosition = 0.8f;

	private float lastInputPositionX = 0;

	private float offSetCorrection = 0.03f;

	private Rigidbody rBody;
	private bool directionLeft;

	float timeToBackRotation = 0;

	public GameController gameController;

	//value to turn jump offset default
	private float JUMP_CALIBRATOR = 7;

	private Animator animationController;

	float velocity = 0;
	private bool dead = false;
	private float timeToJumpDeath = 0;
	private float timetobug = 0;
	// Use this for initialization
	void Start () {
		rBody = this.GetComponent<Rigidbody> ();
		animationController = GetComponent<Animator> ();
		timeToJumpDeath = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (gameController.IsGameRunning ()) {

			//Player Jump
			if (mouseInputJumping ()) {
				playerJump ();
			}

			//Left and Right Player movement
			performPlayerSideMovement ();

			//Check if the player is jumping to disable collider
			disableColliderWhenJumping ();
			dead = false;

			timetobug += Time.deltaTime;

			if (timetobug >= 2)
			{
				animationController.SetBool ("continueGame", false);
			}

			timeToJumpDeath = 0;


			this.GetComponent<SphereCollider> ().radius = 0.5f;
		}

		//control death animation
		if (dead) {
			Debug.Log ("dead");
			timeToJumpDeath += Time.deltaTime;
			animationController.SetBool ("IsJumping", false);
			animationController.SetBool ("isDead", true);

			this.gameObject.GetComponent<SphereCollider> ().isTrigger = false;

			if (timeToJumpDeath > 1)
			{
				this.GetComponent<SphereCollider> ().radius = this.GetComponent<SphereCollider> ().radius / 2f;
				animationController.SetBool ("isDead", false);
				animationController.SetBool ("isDeadJump", true);
				timetobug = 0;
			}

		}
	}

	private void performPlayerSideMovement()
	{
		//If first touch get last input position X
		if (Input.GetMouseButtonDown (0)) {
			lastInputPositionX = Input.mousePosition.x;
		}

		//When draging calculate velocity and direction of the player moviment
		if (Input.GetMouseButton (0)) {
			velocity = Mathf.Abs(Input.mousePosition.x - lastInputPositionX);

			directionLeft = ((Input.mousePosition.x - lastInputPositionX) < 0);

			lastInputPositionX = Input.mousePosition.x;
		}

		//define user experience value to move player velocity
		velocity = velocity / 50;

		//Change the player velocity and rotation when in a moving ground
//		if (gameController.getCurrentGround() != null) {
//			if (gameController.getCurrentGround ().IsMovingGround ())
//				velocity = 0.05f;
//			//this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
//		}


		if (directionLeft) {
			
			movePlayerLeft ();

		} else {
			
			movePlayerRight ();
		}
			
	}
		
	private bool mouseInputJumping()
	{
		return (Input.GetMouseButtonUp (0) && rBody.velocity.y <= 4f);
	}

	private void playerJump()
	{
		animationController.SetBool ("IsJumping",true);
		float velocityadd = rBody.velocity.y;
		rBody.velocity = Vector3.zero;
		rBody.AddForce(new Vector3(0, jumpForce + velocityadd/JUMP_CALIBRATOR , 0), ForceMode.Impulse);
	}

	private void disableColliderWhenJumping()
	{
		if (isGoingUp ()) {
			this.GetComponent<SphereCollider> ().isTrigger = true;
		} else if(this.lastYPosition > this.transform.position.y){
			this.GetComponent<SphereCollider> ().isTrigger = false;
			animationController.SetBool ("IsJumping",false);
		}
		lastYPosition = this.transform.position.y;
	}

	private bool isGoingUp()
	{
		return (this.lastYPosition < this.transform.position.y - offSetCorrection);
	}

	private void movePlayerLeft()
	{
		//walk and prevent get out of wall
		if (this.transform.position.x - velocity > -5) {
			this.transform.position = new Vector3 (this.transform.position.x - velocity, this.transform.position.y,
				this.transform.position.z);
		}

		//reset rotation when stopped
		if (velocity == 0) {
			timeToBackRotation += 0.4f;
			if (timeToBackRotation > 4)
				this.transform.rotation = Quaternion.AngleAxis (180, Vector3.up);
		} else {
			//rotate player
			if (velocity > 0.1f)
				this.transform.rotation = Quaternion.AngleAxis (225, Vector3.up);
			timeToBackRotation = 0;
		}
	}

	private void movePlayerRight()
	{
			//walk and prevent get out of wall
			if (this.transform.position.x + velocity < 5) {
				this.transform.position = new Vector3 (this.transform.position.x + velocity, this.transform.position.y,
					this.transform.position.z);
			}

			//reset rotation when stopped
			if (velocity == 0) {
				timeToBackRotation += 0.4f;
				if(timeToBackRotation > 4)
					this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);

			} else {

				//rotate player
				if (velocity > 0.1f) 
					this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);

				timeToBackRotation = 0;
			}
	}

	public void die()
	{
		dead = true;
		this.transform.rotation = Quaternion.AngleAxis (180, Vector3.up);
	}

}
