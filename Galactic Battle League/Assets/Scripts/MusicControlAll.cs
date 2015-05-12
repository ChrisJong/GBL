using UnityEngine;
using System.Collections;

public class MusicControlAll : MonoBehaviour {

	public AudioSource pirate;
	public AudioSource hiTech;
	public AudioSource military;
	public AudioSource industrial;

	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;


	public float maxVol;
	
	// Update is called once per frame
	void Update () {
		pirate.volume = (float)score1.score/(float)4.0 * maxVol;
		hiTech.volume = (float)score2.score/(float)4.0 * maxVol;
		military.volume = (float)score3.score/(float)4.0 * maxVol;
		industrial.volume = (float)score4.score/(float)4.0 * maxVol;
	}
}
