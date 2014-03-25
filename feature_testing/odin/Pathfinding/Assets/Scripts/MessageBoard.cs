using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageBoard : MonoBehaviour
{
	Rect msgOutRect = new Rect (Screen.width/2 - 200, Screen.height - 450, 400, 375);
	bool boardActive = false;

	Queue<MessagePost> msgQueue = new Queue<MessagePost>();
	string msgTotal;

	AudioPresets makeSound;

	void Start()
	{
		makeSound = gameObject.GetComponent<AudioPresets>();
	}
	
	void Update ()
	{
		if (msgQueue.Count > 0)
		{
			boardActive = true;
			msgTotal = "";
			MessagePost tmp = msgQueue.Peek();

			if (tmp.RemovalTimer < 0)
				msgQueue.Dequeue();

			foreach(MessagePost post in msgQueue)
			{
				msgTotal += post.Message + "\n";

				post.RemovalTimer -= Time.deltaTime;
			}
		}
		else
		{
			boardActive = false;
		}
	}

	public void Post(string message)
	{
		msgQueue.Enqueue (new MessagePost(message));

		AudioClip sound;
		if (makeSound.presets.TryGetValue (message, out sound))
		{
			audio.clip = sound;
			audio.Play();
		}
	}

	void OnGUI()
	{
		if(boardActive)
		{
			GUI.TextArea(msgOutRect, msgTotal);
		}
	}
}
