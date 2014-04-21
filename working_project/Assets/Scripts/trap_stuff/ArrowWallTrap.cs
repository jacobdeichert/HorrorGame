using UnityEngine;
using System.Collections;

public class ArrowWallTrap : Trap {


    //public GameObject[] SpawnPoint; 
    float y = 0;
    float time = 0;
    float Amplitude = 3;
    float Velocity = 0;
    float accleration = 0.0f;
    float frequecy = 0.0f;
    float Period = 0.0f;
    float Omega = 3.0f;
    float phaseAngle = 0.0f;
    public Rigidbody bullet;
    public Transform[] bulletobj;
    void Start() {
   
       
        activate();
        
    }



	void Update() {
        if (state == TrapState.BEGUN && !hasInvokedEnd) {
            hasInvokedEnd = true;
            bulletobj = new Transform[6];
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
          //  SpawnPoint = new GameObject[6];

            FixedUpdate();
            Debug.Log(y);
            if (y == 2.0f || y <= 2.9f)
            {
             Rigidbody arrow =  Instantiate(bullet, bulletobj[0].transform.position, Quaternion.identity) as Rigidbody;
             arrow.AddRelativeForce(bulletobj[0].transform.position);
            } else if (y == 1.0f || y <= 1.9f)
            {
             Rigidbody arrow =  Instantiate(bullet, bulletobj[1].transform.position, Quaternion.identity) as Rigidbody;
             arrow.AddRelativeForce(bulletobj[1].transform.position);
            }else if (y == 0.0f || y <= 0.9)
            {
               Rigidbody arrow = Instantiate(bullet, bulletobj[2].transform.position, Quaternion.identity) as Rigidbody;
               arrow.AddRelativeForce(bulletobj[2].transform.position);
            }else if (y == -0.1f || y <= -0.9f)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[3].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[3].transform.position);
            }else if (y == -1.0 || y <= -1.9 )
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[4].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[4].transform.position);
            }else if (y == -2.0 || y <= -2.9)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[5].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[5].transform.position);
            }else if (y == -3) { }
            {
            } 
		}
	}



	public override void end() {
        if (state == TrapState.BEGUN) {
            base.end();
		    
        }
	}
   public void FixedUpdate()
   {
       y = 0;
       time += Time.deltaTime;
       Period = 2.0f * 3.14f / Omega;
       frequecy = 1 / Period;
       y += Amplitude * Mathf.Sin(Omega * time + phaseAngle);
       Velocity += Omega * Amplitude * Mathf.Cos(Omega * time + phaseAngle);
       accleration += -Omega * Omega * y;
     
       //Debug.Log(y);
   }
   
}









