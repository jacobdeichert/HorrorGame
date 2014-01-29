using UnityEngine;
using System.Collections;

public class FloorSectionTrigger : MonoBehaviour {

	GameObject floorSection;
	string trapType;
	bool isActivated = false;
	
	void Start () {
		floorSection = gameObject.transform.parent.gameObject;
		trapType = floorSection.name;
	}

	
	void OnTriggerEnter(Collider other) {
		if (other.name == "player" && !isActivated) {

			// lower the trigger platform that the player stepped on
			gameObject.transform.position += new Vector3(0, -0.2f, 0);

			if (trapType == "floor_section_crushing_wall_trap") {
				CrushingWallTrap trap = floorSection.GetComponentInChildren<CrushingWallTrap>() as CrushingWallTrap;
				trap.activate();
			} else if (trapType == "floor_section_falling_floor_trap") {
				FallingFloorTrap trap = floorSection.GetComponentInChildren<FallingFloorTrap>() as FallingFloorTrap;
				trap.activate();
			}

			isActivated = true;
		}
	}
}
