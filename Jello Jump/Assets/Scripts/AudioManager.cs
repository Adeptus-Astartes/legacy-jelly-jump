using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour 
{
	public List<AudioSource> tracks;
	private int currentTrack = 0;

	void Start()
	{
		currentTrack = Random.Range(0,tracks.Count);
		tracks[currentTrack].Play();
	}

	void Update () 
	{
		if(!tracks[currentTrack].isPlaying)
		{

			currentTrack = Random.Range(0,tracks.Count);
		    tracks[currentTrack].Play();
		}
	}
}
