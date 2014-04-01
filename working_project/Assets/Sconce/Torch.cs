using UnityEngine;
using System.Collections;

public class Torch : MonoBehaviour {

	private bool flameOn = true;

	// Use this for initialization
	void Start () {
		ToggleFlame();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F)){
			ToggleFlame();
		}
	}

	public void ToggleFlame(){
		if (flameOn){
			flameOn = false;
			transform.Find("Fire").particleSystem.enableEmission = false;
			transform.Find("Fire Light").light.enabled = false;
		}
		else {
			flameOn = true;
			transform.Find("Fire").particleSystem.enableEmission = true;
			transform.Find("Fire Light").light.enabled = true;
		}
	}
}
