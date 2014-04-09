﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (BoxCollider))]

public class PlayerController : MonoBehaviour {
	
	public float walkSpeed;
	public float runSpeed;
	public float strafeSpeed;
	public float gravity;
	public float jumpHeight;
	public bool canJump;
	private bool isRunning = false;
    private bool isGrounded = false;

	void Awake () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}
	
	void FixedUpdate () {
		// get correct speed
		float forwardAndBackSpeed = walkSpeed;

        // if running, set run speed
		if (isRunning) {
			forwardAndBackSpeed = runSpeed;
		}

		// calculate how fast it should be moving
		Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * strafeSpeed, 0, Input.GetAxis("Vertical") * forwardAndBackSpeed);
		targetVelocity = transform.TransformDirection(targetVelocity);
		
		// apply a force that attempts to reach our target velocity
		Vector3 velocity = rigidbody.velocity;
		Vector3 velocityChange = (targetVelocity - velocity);
		velocityChange.y = 0;
		rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
		
		// jump
		if (canJump && isGrounded && Input.GetButton("Jump")) {
			rigidbody.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
			isGrounded = false;
		}
		
		// apply gravity
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
	}

	void Update() {
        // check if the player is touching a surface below them
        checkGrounded();

        // check if the player is running
		if (isGrounded && Input.GetButtonDown("Sprint")) {
			isRunning = true;
		}
		
        // check if the player stops running
		if (Input.GetKeyUp(KeyCode.LeftShift)) {
			isRunning = false;
		}
	}
	
	void checkGrounded() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position - new Vector3(0, 0.9f, 0), -transform.up);
        // if there is something directly below the player
        if (Physics.Raycast(ray, out hit, 0.15f)) {
            isGrounded = true;
        }
	}


}
