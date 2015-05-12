using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicControlLeader : MonoBehaviour {

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
		Dictionary<int, int> scores = new Dictionary<int, int>();

		scores.Add(1, score1.score);
		scores.Add(2, score2.score);
		scores.Add(3, score3.score);
		scores.Add(4, score4.score);

		KeyValuePair<int, int> leader = new KeyValuePair<int, int>();
		foreach (KeyValuePair<int, int> kvp in scores){
			if (kvp.Value > leader.Value)
				leader = kvp;
		}

		if (leader.Key == 1) {
			pirate.volume = (float)leader.Value/(float)5.0 * maxVol;
			hiTech.volume = 0;
			military.volume = 0;
			industrial.volume = 0;
		} else if (leader.Key == 2) {
			pirate.volume = 0;
			hiTech.volume = (float)leader.Value/(float)5.0 * maxVol;
			military.volume = 0;
			industrial.volume = 0;
		} else if (leader.Key == 3) {
			pirate.volume = 0;
			hiTech.volume = 0;
			military.volume = (float)leader.Value/(float)5.0 * maxVol;
			industrial.volume = 0;
		} else {
			pirate.volume = 0;
			hiTech.volume = 0;
			military.volume = 0;
			industrial.volume = (float)leader.Value/(float)5.0 * maxVol;
		}

	}
}
