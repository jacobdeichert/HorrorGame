using UnityEngine;
using System.Collections;

public class CrushingWallTrap : Trap {



    void Start() {
        activate();
    }



	void Update() {
        if (state == TrapState.BEGUN && !hasInvokedEnd) {
            hasInvokedEnd = true;
            Invoke("end", 10f);
        }
	}



    public override void correctRotation() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);
        bool hasHitSomething = Physics.Raycast(ray, out hit, 2.0f);
        int testCount = 4;

        while (hasHitSomething) {
            gameObject.transform.parent.gameObject.transform.Rotate(0, 90, 0);
            ray = new Ray(transform.position, transform.forward);
            hasHitSomething = Physics.Raycast(ray, out hit, 2.0f);

            // stop the loop after 4 tries (trap is blocked on all sides)
            testCount--;
            if (testCount <= 0) break;
        }

        // check if there is a floor trap in front of the wall trap
        ray = new Ray(transform.position + transform.up * -3, transform.forward);
        if (Physics.Raycast(ray, out hit, 1.0f)) {
            if (hit.collider.transform.parent
                && (hit.collider.transform.parent.GetComponentInChildren<SpikeFloorTrap>()) as SpikeFloorTrap) {
                // remove the floor trap trigger so that it doesn't interfere with the wall trap trigger
                // the floor trap will never be activated
                Destroy(hit.collider.transform.parent.GetChild(2).gameObject);
            }
        }
    }



    public override void begin() {
		if (state == TrapState.IDLE && isActivated) {
            base.begin();
            constantForce.enabled = true;
		}
	}



	public override void end() {
        if (state == TrapState.BEGUN) {
            base.end();
		    constantForce.enabled = false;
		    rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
	}
}









