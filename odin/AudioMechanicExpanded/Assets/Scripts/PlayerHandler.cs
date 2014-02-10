using UnityEngine;
using System.Collections;

public class PlayerHandler : MonoBehaviour
{
	MessageBoard msgBoard;

	bool typeReady = false;
	Rect msgInRect = new Rect(Screen.width/2 - 200, Screen.height - 50, 400, 20);
	string msg = "";

	void Start ()
	{
		msgBoard = gameObject.GetComponent<MessageBoard> ();
	}

	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Return) && typeReady == false)
			typeReady = true;
	}
	void OnGUI()
	{
		if(typeReady)
		{
			// Create input area and focus on it
			GUI.SetNextControlName("MsgIn");
			msg = GUI.TextField(msgInRect, msg);
			GUI.FocusControl("MsgIn");
			
			// Detect return key to send message
			if(Event.current.isKey && Event.current.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "MsgIn")
			{
				typeReady = false;
				msgBoard.Post(msg);
				msg = "";
			}
		}
	}
}
