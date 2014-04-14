using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPresets : MonoBehaviour
{
	public SortedDictionary<string, AudioClip> presets = new SortedDictionary<string, AudioClip>();

	public AudioClip help;
	public AudioClip overHere;
	public AudioClip runAway;
    public AudioClip watchOut;

	void Start ()
	{
        presets.Add("help", help);
        presets.Add("over here", overHere);
        presets.Add("run away", runAway);
        presets.Add("watch out", watchOut);
	}
}
