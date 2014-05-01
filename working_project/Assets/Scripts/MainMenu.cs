using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Screen.showCursor = true;
        Screen.lockCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
	     if (Input.GetButton("Jump")) {
             Application.LoadLevel("Level1");
         } else if (Input.GetButton("Back")) {
             Application.Quit();
         }
	}
}
