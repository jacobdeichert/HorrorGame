using UnityEngine;
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
	bool isRunning = false;
	bool isGrounded = false;

	void Awake () {
		rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
	}
	
	void FixedUpdate () {
		// get correct speed
		float forwardAndBackSpeed = walkSpeed;

		if (isGrounded && Input.GetKey(KeyCode.LeftControl)) {
			isRunning = true;
		}

		if (Input.GetKeyUp(KeyCode.LeftControl)) {
			isRunning = false;
		}

		if (isRunning) {
			forwardAndBackSpeed = runSpeed;
		}

		// calculate how fast we should be moving
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
		}
		
		// apply gravity
		rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		isGrounded = false;
	}
	
	void OnCollisionStay () {
		isGrounded = true;
	}


}
