using UnityEngine;
using System.Collections;

public class ObjectSounds : MonoBehaviour
{
	void Start ()
	{
	
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1) && gameObject.name == "Sphere")
			audio.Play ();	
		else if (Input.GetKeyDown (KeyCode.Alpha2) && gameObject.name == "Capsule")
			audio.Play ();	
		else if (Input.GetKeyDown (KeyCode.Alpha3) && gameObject.name == "Cylinder")
			audio.Play ();	
		else if (Input.GetKeyDown (KeyCode.Alpha4) && gameObject.name == "Cube")
			audio.Play ();	
	}
}
