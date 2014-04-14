using UnityEngine;
using System.Collections;

public struct SoundSourceInfo
{
	public Transform transform;
	public float volume;
};
public class AudioOutputToEnemy : MonoBehaviour
{
	SoundSourceInfo soundInfo;
	GameObject monster;

	void Start ()
	{
		monster = GameObject.FindGameObjectWithTag("Monster");
	}

	void Update ()
	{
		if(audio.isPlaying && audio.name != "ambience")
		{
			soundInfo.transform = transform;
			soundInfo.volume = audio.volume;
			monster.SendMessage("OnHearSound", soundInfo, SendMessageOptions.RequireReceiver);
		}
	}
}
