using UnityEngine;
using System.Collections;

public class PendulumTrap : Trap {
	
	private float W = Mathf.Sqrt(9.8f / 1.5f);
	private float D = Mathf.PI / 2;
	private float MAX_ANGLE;// = transform.rotation.z * Mathf.PI / 180;
	private float angle;
	private float time;
	
	void Start() {
		time = Time.time;
		MAX_ANGLE = transform.rotation.z * Mathf.PI / 180;
		activate();
	}
	
	
	
	void Update() {
		if (state == TrapState.BEGUN && !hasInvokedEnd) {
			//hasInvokedEnd = true;
			//end();

		}
		time += Time.deltaTime;
		angle = MAX_ANGLE * Mathf.Sin(W * time + D);
		angle *= 180 / Mathf.PI;
		
		Quaternion rot = transform.rotation;
		rot.z = angle;
		transform.rotation = rot;
	}
	
	
	public override void begin() {
		if (state == TrapState.IDLE && isActivated) {
			base.begin();
			//transform.position += new Vector3(0, 1, 0);

		}
	}
	
	
	
	public override void end() {
		if (state == TrapState.BEGUN) {
			base.end();
			
		}
	}
}