using UnityEngine;
using System.Collections;

public class FlickeringLight : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	
	}
    void FixedUpdate()
    {
        gameObject.light.intensity = Random.Range(1.0f, 2.0f);
    }
}
