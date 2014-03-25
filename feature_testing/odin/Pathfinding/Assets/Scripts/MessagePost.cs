using UnityEngine;
using System.Collections;

public class MessagePost
{
	string message;
	public string Message
	{
		get {return message;}
	}
	
	float removalTimer = 5.0f;
	public float RemovalTimer
	{
		get { return removalTimer;}
		set { removalTimer = value;}
	}

	public MessagePost(string message)
	{
		this.message = message;
	}
}
