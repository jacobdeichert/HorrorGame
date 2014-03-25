using UnityEngine;
using System.Collections;

public class SpikeFloorTrap : Trap {



    void Start() {
        activate();
    }



    void Update() {
        if (state == TrapState.BEGUN && !hasInvokedEnd) {
            hasInvokedEnd = true;
            end();
        }
    }

    
    public override void begin() {
        if (state == TrapState.IDLE && isActivated) {
            base.begin();
            transform.position += new Vector3(0, 1, 0);
        }
    }



    public override void end() {
        if (state == TrapState.BEGUN) {
            base.end();

        }
    }
}
