using UnityEngine;
using System.Collections;

public class ArrowWallTrap : Trap {


    //public GameObject[] SpawnPoint; 
    float y = 0;
    float time =0;
    float Amplitude = -2.0f;
   public float pos = -1.0f;
    float Velocity;
    float accleration;
    float frequecy ;
    float Period ;
    float Omega ;
    float phaseAngle;
    float sine;
    float totalsine;
    public GameObject[] bullet = new GameObject[6];
    private GameObject[] b = new GameObject[6];
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
            

            for(int i = 0; i < 6; i ++)
            {

          b[i] =   Instantiate(bullet[i], bulletobj[i].position, Quaternion.identity) as GameObject;
            
            }
           Debug.Log("Started");
        
          
		}
	}



	public override void end() {
        if (state == TrapState.BEGUN) {
            base.end();
            for (int i = 0; i < 6; i++) 
            {
                b[i].SetActive(false);
            }
        }
	}
   public void FixedUpdate()
   {
       
       if (state == TrapState.BEGUN)
       {
           
           float siney = Mathf.Sin(Period * time + phaseAngle ) * Amplitude;
           float cosv = Mathf.Cos(Period * time + phaseAngle) * Amplitude;
           float negsina = -Mathf.Sin(Omega * time + phaseAngle) * Amplitude;
           float totalsine = siney * cosv * negsina * time;
           time += Time.deltaTime;
           Omega = 2.0f * 3.14f / time;
          // phaseAngle = 1;
           Period = 2.0f * 3.14f / Omega;
           frequecy = 1 / Period;
           for(int i = 0;i< 6; i++){
           b[i].transform.Translate(0.025f, totalsine*frequecy , 0);
           }
               Debug.Log(totalsine);
       }
       if (state == TrapState.ENDED) 
       {
        
       }    
   }
   
}









