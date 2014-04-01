using UnityEngine;
using System.Collections;

public class Globals : MonoBehaviour {


    // Make this game object and all its transform children
    // survive when loading a new scene.
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
