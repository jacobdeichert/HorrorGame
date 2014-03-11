using UnityEngine;
using System.Collections;

public class FlashLight : MonoBehaviour {
    GameObject obj;
    float flashlight;
	double time = 20;
	bool ison;
	// Use this for initialization
	void Start () {

        flashlight = 0.0f;
		obj.light.intensity = flashlight;
		ison = true;
	}
	
	// Update is called once per frame
	void Update () {
        while (ison == true) 
        {
            if (Input.GetButtonDown("FlashLight")) 
            {
                obj.light.intensity = flashlight + 1;
            }
         
        }	
	}

}
