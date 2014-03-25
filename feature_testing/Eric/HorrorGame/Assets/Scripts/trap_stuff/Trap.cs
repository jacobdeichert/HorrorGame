using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour
{
    public enum TrapState { 
        IDLE,
        BEGUN,
        ENDED
    }
    public TrapState state = TrapState.IDLE;
	public bool isActivated = false;
    public bool hasInvokedEnd = false;


    public virtual void correctRotation() {
    }


    public virtual void activate() { 
        isActivated = true;
    }



    public virtual void deactivate() {
        isActivated = false;
    }



    public virtual void begin() {
        state = TrapState.BEGUN;
	}



    public virtual void end() {
        state = TrapState.ENDED;
	}
}

