using UnityEngine;
using System.Collections;

[RequireComponent (typeof (AudioSource))]
public class MusicVolumeController : MonoBehaviour {
	public static float musicVolume = 1.0f;
	public static bool musicActive = true;
	AudioSource music;

	// Use this for initialization
	void Start () {
		music = GetComponent<AudioSource>();
		WinScreenMusicController winScreenMusic = GameObject.FindObjectOfType<WinScreenMusicController> ();

		if (winScreenMusic != null) {
			winScreenMusic.StopMusic();
			Destroy (winScreenMusic);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (MusicVolumeController.musicActive) {
			music.volume = MusicVolumeController.musicVolume;
		} else {
			music.volume = 0;
		}

		if (Input.GetKeyDown(KeyCode.M))
			MusicVolumeController.ToggleMusic();
	}

	public static void ToggleMusic() {
		musicActive = !musicActive;
		Debug.Log("Music Enabled: " + musicActive);
	}
}
