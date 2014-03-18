using UnityEngine;
using System.Collections;

public class TrapTrigger : MonoBehaviour {

	GameObject trapPrefab;
	//string trapType;
	bool isActivated = false;
	
	void Start () {
        trapPrefab = gameObject.transform.parent.gameObject;
        //trapType = trapPrefab.name;
	}

	
	void OnTriggerEnter(Collider other) {
        // if the thing that triggered it was a player
		if ((PlayerController)(other.GetComponent<PlayerController>()) && !isActivated) {
			// lower the trigger platform that the player stepped on
			gameObject.transform.position += new Vector3(0, -0.05f, 0);

            // start the trap
            Trap trap = trapPrefab.GetComponentInChildren<Trap>() as Trap;
            trap.begin();

			//if (trapType == "trap_crushing_wall") {
				//CrushingWallTrap trap = floorSection.GetComponentInChildren<CrushingWallTrap>() as CrushingWallTrap;
                
			//} else if (trapType == "floor_section_falling_floor_trap") {
				//FallingFloorTrap trap = floorSection.GetComponentInChildren<FallingFloorTrap>() as FallingFloorTrap;
				//trap.activate();
			//}

			isActivated = true;
		}
	}
}
