using UnityEngine;
using System.Collections;

public class MusicControlAll : MonoBehaviour {
	
	public AudioSource pirate;
	public AudioSource hiTech;
	public AudioSource military;
	public AudioSource industrial;
	
	public ScoreCounter score1;
	public ScoreCounter score2;
	public ScoreCounter score3;
	public ScoreCounter score4;
	
	public float maxVolPirate = 0.4f;
	public float maxVolTech = 0.6f;
	public float maxVolMilitary = 0.8f;
	public float maxVolIndustrial = 0.6f;

	void Start()
	{
		GameObject.Destroy (GameObject.Find ("MenuMusic"));
	}

	// Update is called once per frame
	void Update () {
		if (MusicVolumeController.musicActive) {
			pirate.volume = Mathf.Clamp((float)score1.score/4.0f * maxVolPirate * MusicVolumeController.musicVolume, 0, maxVolPirate * MusicVolumeController.musicVolume);
			hiTech.volume = Mathf.Clamp((float)score2.score/4.0f * maxVolTech * MusicVolumeController.musicVolume, 0, maxVolTech * MusicVolumeController.musicVolume);
			military.volume = Mathf.Clamp((float)score3.score/4.0f * maxVolMilitary * MusicVolumeController.musicVolume, 0, maxVolMilitary * MusicVolumeController.musicVolume);
			industrial.volume = Mathf.Clamp((float)score4.score/4.0f * maxVolIndustrial * MusicVolumeController.musicVolume, 0, maxVolIndustrial * MusicVolumeController.musicVolume);
		} else {
			pirate.volume = 0;
			hiTech.volume = 0;
			military.volume = 0;
			industrial.volume = 0;
		}
	}
}
