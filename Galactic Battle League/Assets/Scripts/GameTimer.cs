using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {

	public float time { get; set;}
	Text gameTime;

	// Use this for initialization
	void Start () {
		time = 300;
		gameTime = GetComponent<Text> ();
		InvokeRepeating ("DecreasingTime", 1, 1);
		gameTime.text = DisplayTime ();
	}
	
	// Update is called once per frame
	void Update () {
		gameTime.text = DisplayTime ();
	}

	void IncreasingTime()
	{
		time++;
	}

	void DecreasingTime()
	{
		if (time > 0)
			time--;
		else
			time = 0;
	}

	string DisplayTime()
	{
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;

		return string.Format ("{0:0}", minutes) + ":" + string.Format ("{0:00}", seconds);
	}
}
