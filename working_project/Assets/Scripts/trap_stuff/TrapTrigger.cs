using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour {

	private GameObject trapPrefab;
	private bool isActivated = false;
	
	void Start () {
        trapPrefab = gameObject.transform.parent.gameObject;
	}

	
	void OnTriggerEnter(Collider other) {
        // if the thing that triggered it was a player
		if (!isActivated && (PlayerController)(other.GetComponent<PlayerController>())) {
			// lower the trigger platform that the player stepped on
			gameObject.transform.position += new Vector3(0, -0.05f, 0);

            // start the trap
            Trap trap = trapPrefab.GetComponentInChildren<Trap>() as Trap;
            trap.begin();

			isActivated = true;
		}
	}
}
