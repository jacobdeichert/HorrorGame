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









