using UnityEngine;
using System.Collections;

public class CrushingWallTrap : MonoBehaviour {

	bool isActivated = false;


	void Update() {
		if (Mathf.Round(transform.localPosition.x) == 1) {
			Invoke("stop", 2f);
		}
	}
		
	public void activate() {
		if (!isActivated) {
			constantForce.enabled = true;
			isActivated = true;
		}
	}

	void stop() {
		constantForce.enabled = false;
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
	}
}









