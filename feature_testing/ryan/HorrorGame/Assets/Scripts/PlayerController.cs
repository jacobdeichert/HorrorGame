using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody))]


public class PlayerController : MonoBehaviour {
	public string actions = "idle";
	//public int indexAction;						// what action should be played
	
	public GUIText statusGUI;					// where the action is displayed
	private Vector3 idlepoint;
	public float animationTime = 1.0f;				// how long animations take
	public float walkSpeed;
	public float runSpeed;
	public float strafeSpeed;
	public float gravity;
	public float jumpHeight;
	public bool canJump;
	bool isRunning = false;
	bool isGrounded = false;
	bool isIdle = false;
	void Awake () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;

		animation.Play("idle");
	}
	
	void FixedUpdate () {
		// get correct speed
		float forwardAndBackSpeed = walkSpeed;
		//idlepoint = new Vector3(0,0,0);
		
		if (isRunning)
		{
			forwardAndBackSpeed = runSpeed;
			animation["run"].speed = animationTime ;
			animation.CrossFade("run");
		}
		else if (!isRunning)
		{
			animation["run"].speed = 0.5f;
			animation.CrossFade("run");
		}
	   

		// calculate how fast we should be moving
		if (!isIdle)
		{
			Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal") * strafeSpeed, 0, Input.GetAxis("Vertical") * forwardAndBackSpeed);
			targetVelocity = transform.TransformDirection(targetVelocity);
			animation["run"].speed = animationTime / 2.0f;
			animation.Play("run");
			// apply a force that attempts to reach our target velocity
			Vector3 velocity = rigidbody.velocity;
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.y = 0;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			if (canJump && isGrounded && Input.GetButton("Jump"))
			{
				rigidbody.velocity = new Vector3(velocity.x, Mathf.Sqrt(2 * jumpHeight * gravity), velocity.z);
				isGrounded = false;
			}
		}
		 else if (isIdle) { animation.Play("idle"); }
		// jump
		
		
		// apply gravity
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		//isGrounded = false;
	}

	void Update()
	{
		if (isGrounded && Input.GetKeyDown(KeyCode.LeftControl))
		{
			isRunning = true;

		}

		if (Input.GetKeyUp(KeyCode.LeftControl))
		{
			isRunning = false;

		}
	}
	
	void OnCollisionStay () {
		isGrounded = true;
		//animation.Play("idle");
	}
	void OnIdle() 
	{
		
	}

}
