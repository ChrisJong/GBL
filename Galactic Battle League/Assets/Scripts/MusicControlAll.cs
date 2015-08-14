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
	
	// Update is called once per frame
	void Update () {
		pirate.volume = (float)score1.score/(float)4.0 * maxVolPirate;
		hiTech.volume = (float)score2.score/(float)4.0 * maxVolTech;
		military.volume = (float)score3.score/(float)4.0 * maxVolMilitary;
		industrial.volume = (float)score4.score/(float)4.0 * maxVolIndustrial;
	}
}
