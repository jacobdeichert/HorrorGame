using UnityEngine;
using System.Collections;

public class ServerMaster : MonoBehaviour {

    //Needs to be unique...so why not throw in extra characters
    private const string typeName = "13-HorrorGame-13";
    //Name of the room
    private const string gameName = "OpenedRoom";

    private HostData[] hostList;
	
    // Use this for initialization
	void Start () {
	
	}

    public void StartServer()
    {
        Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
        MasterServer.RegisterHost(typeName, gameName);
    }

    void OnServerInitialized()
    {
        Application.LoadLevel("Level1");
    }

    public void RefreshHostList()
    {
        MasterServer.RequestHostList(typeName);
    }

    void OnMasterServerEvent(MasterServerEvent msEvent)
    {
        if (msEvent == MasterServerEvent.HostListReceived)
            hostList = MasterServer.PollHostList();
    }

    private void JoinServer(HostData hostData)
    {
        Network.Connect(hostData);
    }

    void OnConnectedToServer()
    {
        Debug.Log("Server Joined");
    }

    void OnGUI()
    {
        if (!Network.isClient && !Network.isServer)
        {
            if (hostList != null)
            {
                for (int i = 0; i < hostList.Length; i++)
                {
                    if (GUI.Button(new Rect(400, 100 + (110 * i), 300, 100), hostList[i].gameName))
                        JoinServer(hostList[i]);
                }
            }
        }
    }

	// Update is called once per frame
	void Update () {
	
	}
}
