using UnityEngine;
using System.Collections;

public class JoinClicked : MonoBehaviour {

    private GameObject globals;
    private ServerMaster server;

    // Use this for initialization
    void Start()
    {
        globals = GameObject.FindGameObjectWithTag("Globals");
        server = globals.GetComponent<ServerMaster>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown()
    {
        //Start the server
        server.RefreshHostList();
        GetComponent<ButtonScript>().Disable();
    }
}
