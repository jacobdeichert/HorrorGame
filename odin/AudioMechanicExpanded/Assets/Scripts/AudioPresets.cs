using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPresets : MonoBehaviour
{
	public SortedDictionary<string, AudioClip> presets = new SortedDictionary<string, AudioClip>();

	public AudioClip jump;
	public AudioClip select;
	public AudioClip woosh;

	void Start ()
	{
		presets.Add ("jump", jump);
		presets.Add ("select", select);
		presets.Add ("woosh", woosh);
	}
}
