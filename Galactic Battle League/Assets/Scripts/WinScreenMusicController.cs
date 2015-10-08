using UnityEngine;
using System.Collections;

public class WinScreenMusicController : MonoBehaviour {


	AudioSource audio;

	public AudioClip pirateSting;
	public AudioClip techSting;
	public AudioClip militarySting;
	public AudioClip industrialSting;
	
	public AudioClip music;

	float soundVolume;

	// Use this for initialization
	IEnumerator Start () {
		audio = GetComponent<AudioSource>();
		audio.volume = soundVolume;

		AudioClip sting = pirateSting;

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
		yield return new WaitForSeconds(audio.clip.length);
		audio.volume = 0;
		audio.clip = music;
		audio.Play ();
		fadeIn ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void fadeIn(){
		if (audio.volume < 1) {
			audio.volume += 1 *Time.deltaTime;
		}
	}
}
