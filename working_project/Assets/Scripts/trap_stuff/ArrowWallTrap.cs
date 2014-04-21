using UnityEngine;
using System.Collections;

public class ArrowWallTrap : Trap {


    //public GameObject[] SpawnPoint; 
    float y = 0;
    float time ;
    float Amplitude = 3;
    float Velocity;
    float accleration;
    float frequecy ;
    float Period ;
    float Omega ;
    float phaseAngle;
    float sine;
    float totalsine;
    public Rigidbody bullet;
    public Transform[] bulletobj = new Transform[6];
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
          //  SpawnPoint = new GameObject[6];
            if (totalsine == 2.0f || totalsine <= 2.9f)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[0].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[0].transform.position);
                y -= y;
                Debug.Log(y);
            }
            else if (totalsine == 1.0f || totalsine <= 1.9f)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[1].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[1].transform.position);
                Debug.Log(y);
            }
            else if (totalsine == 0.0f || totalsine <= 0.9)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[2].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[2].transform.position);
                Debug.Log(y);
            }
            else if (totalsine == -0.1f || totalsine <= -0.9f)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[3].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[3].transform.position);
            }
            else if (totalsine == -1.0 || totalsine <= -1.9)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[4].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[4].transform.position);
            }
            else if (y == -2.0 || y <= -2.9)
            {
                Rigidbody arrow = Instantiate(bullet, bulletobj[5].transform.position, Quaternion.identity) as Rigidbody;
                arrow.AddRelativeForce(bulletobj[5].transform.position);
            }
            else if (y == -3) { }
            {
               
            } 
            
            Debug.Log(y);
          
		}
	}



	public override void end() {
        if (state == TrapState.BEGUN) {
            base.end();
		    
        }
	}
   public void FixedUpdate()
   {
       
       if (state == TrapState.BEGUN)
       {
           float sine = Mathf.Sin(Period * time + phaseAngle ) * Amplitude;
           float totalsine = sine + sine;
           time += Time.deltaTime;
           Omega = 2.0f * 3.14f / time;
          // phaseAngle = 1;
           Period = 2.0f * 3.14f / Omega;
           frequecy = 1 / Period;
         
           Debug.Log(totalsine);
       }
       if (state == TrapState.ENDED) 
       {
        
       }    
   }
   
}









