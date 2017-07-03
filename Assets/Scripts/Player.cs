﻿using System.Collections;
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

	// Use this for initialization
	void Start () {
		rBody = this.GetComponent<Rigidbody> ();
		animationController = GetComponent<Animator> ();
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
			
		} else {
		
			if (Input.GetMouseButtonUp (0)) {
				Application.LoadLevel ("Main");			
			}
		}
	}

	private void performPlayerSideMovement()
	{
		
		if (Input.GetMouseButtonDown (0)) {
			lastInputPositionX = Input.mousePosition.x;
		}
		if (Input.GetMouseButton (0)) {
			velocity = Mathf.Abs(Input.mousePosition.x - lastInputPositionX);

			directionLeft = ((Input.mousePosition.x - lastInputPositionX) < 0);

			lastInputPositionX = Input.mousePosition.x;
		}
		
		velocity = velocity / 50;


		if (directionLeft) {

			//walk and prevent get out of wall
			if (this.transform.position.x - velocity > -7) {
				this.transform.position = new Vector3 (this.transform.position.x - velocity, this.transform.position.y,
					this.transform.position.z);
			}

			//reset rotation when stopped
			if (velocity == 0) {
				timeToBackRotation += 0.4f;
				if(timeToBackRotation > 4)
					this.transform.rotation = Quaternion.AngleAxis(180,Vector3.up);
			} else {
				//rotate player
				if (velocity > 0.2f) 
				this.transform.rotation = Quaternion.AngleAxis(225,Vector3.up);
				timeToBackRotation = 0;
			}

		}
		else {
			//walk and prevent get out of wall
			if (this.transform.position.x + velocity < 7) {
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
				if (velocity > 0.2f) 
				this.transform.rotation = Quaternion.AngleAxis(135,Vector3.up);

				timeToBackRotation = 0;
			}
		}
	}

	private bool mouseInputJumping()
	{
		return (Input.GetMouseButtonUp (0) && rBody.velocity.y <= 3f);
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
}
