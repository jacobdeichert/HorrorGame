using UnityEngine;
using System.Collections;

public class FallingFloorTrap : MonoBehaviour {

	bool isActivated = false;

	public void activate() {
		if (!isActivated) {
			gameObject.collider.enabled = false;
			gameObject.constantForce.enabled = true;
			gameObject.rigidbody.useGravity = true;
			isActivated = true;
			Invoke("hide", 5f);
		}
	}

	void hide() {
		Destroy(gameObject.constantForce);
		Destroy(gameObject.rigidbody);
		gameObject.renderer.enabled = false;
	}
}
