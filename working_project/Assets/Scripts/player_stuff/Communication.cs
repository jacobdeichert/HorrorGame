using UnityEngine;
using System.Collections;

public class Communication : MonoBehaviour
{
	MessageBoard msgBoard;
	
	bool typeReady = false;
	Rect msgInRect = new Rect(Screen.width/2 - 200, Screen.height - 50, 400, 20);
	string msg = "";
	
	const int BTN_W_H = 30;
	const int TXT_LINE_H = 20;
	const int VOL_TXT_LINE_W = 100;
	Rect volMsgLabelRect = new Rect(30, Screen.height - 50, VOL_TXT_LINE_W, TXT_LINE_H); 
	Rect volDownBtnRect;
	Rect volUpBtnRect;
	Rect volLabelRect;
	public Texture volDownBtnTex;
	public Texture volUpBtnTex;
	AudioSource voice;
	string volMsg = "";
	
	void Start ()
	{
		msgBoard = gameObject.GetComponent<MessageBoard> ();
		
		// These use each other's properties in this sequence.
		volDownBtnRect = new Rect (volMsgLabelRect.x, volMsgLabelRect.y - (BTN_W_H + 10), BTN_W_H, BTN_W_H);
		volUpBtnRect = new Rect (volDownBtnRect.x + (BTN_W_H + 10), volDownBtnRect.y, BTN_W_H, BTN_W_H);
		volLabelRect = new Rect (volDownBtnRect.x, volDownBtnRect.y - (TXT_LINE_H + 10), VOL_TXT_LINE_W, TXT_LINE_H);
		
		audio.volume = 0.5f;
	}
	
	void Update ()
	{
		if (Input.GetKeyUp (KeyCode.Return) && typeReady == false)
			typeReady = true;
		
		if ((audio.volume*100) > 79)
			volMsg = "Shout";
		else if ((audio.volume*100) > 59 && (audio.volume*100) < 80)
			volMsg = "Yell";
		else if ((audio.volume*100) > 39 && (audio.volume*100) < 60)
			volMsg = "Moderate";
		else if ((audio.volume*100) > 19 && (audio.volume*100) < 40)
			volMsg = "Dull";
		else
			volMsg = "Whisper";
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
		
		GUI.Label (volLabelRect, "Voice Volume");
		if(GUI.RepeatButton (volDownBtnRect, volDownBtnTex) || Input.GetKey(KeyCode.Minus))
			audio.volume -= 0.001f;
		if(GUI.RepeatButton (volUpBtnRect, volUpBtnTex) || Input.GetKey(KeyCode.Equals))
			audio.volume += 0.001f;
		GUI.Label (volMsgLabelRect, "" + Mathf.RoundToInt((audio.volume*100)) + ", " + volMsg);
	}
}
