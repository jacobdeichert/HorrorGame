using UnityEngine;
using System.Collections;

public class HostClicked : MonoBehaviour {

    private GameObject globals;
    private ServerMaster server;
	// Use this for initialization
	void Start () {
        globals = GameObject.FindGameObjectWithTag("Globals");
        server = globals.GetComponent<ServerMaster>();
	}
	
	// Update is called once per frame
	void Update () {
       
	}

    void OnMouseDown()
    {
        //Start the server
        server.StartServer();
        GetComponent<ButtonScript>().Disable();
    }
}
