using UnityEngine;
using System.Collections;

public class WinScreenMusicController : MonoBehaviour {

	AudioSource audio;
	float audio2volume;
	AudioClip sting;

	public AudioClip pirateSting;
	public AudioClip techSting;
	public AudioClip militarySting;
	public AudioClip industrialSting;
	public AudioClip music;
	public float soundVolume;
	public float musicMaxVolume;
	public float fadeTime;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
		audio2volume = musicMaxVolume;
		audio = GetComponent<AudioSource>();
		audio.volume = soundVolume;
		
		sting = pirateSting;
		
		int winner = PlayerPrefs.GetInt ("Position1Player");
		
		switch (winner) {
		case 1: 
			sting = pirateSting;
			break;
		case 2:
			sting = techSting;
			break;
		case 3:
			sting = militarySting;
			break;
		case 4:
			sting = industrialSting;
			break;
		}
		
		audio.clip = sting;
		audio.Play ();
		Invoke ("fadeIn", sting.length);
	}
	
	// Update is called once per frame
	void Update () {
		if (audio2volume < musicMaxVolume) {
			audio2volume += Time.deltaTime / fadeTime;
			audio.volume = audio2volume;
		}
	}

	void fadeIn(){
		audio2volume = 0;
		audio.volume = 0;
		audio.clip = music;
		
		audio.Play ();
	}

	public void StopMusic()
	{
		audio.Stop ();
	}

}
