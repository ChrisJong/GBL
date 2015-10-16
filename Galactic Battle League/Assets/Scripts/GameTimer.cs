using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameTimer : MonoBehaviour {

	public float time = 300;
	public AudioClip warning = null;
	public AudioClip countdown = null;
	public int warningTime = 30;
	public int countdownTime = 10;
	Text gameTime;

	// Use this for initialization
	void Start () {
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

		if (time == warningTime) {
			AudioSource.PlayClipAtPoint(warning, Vector3.zero);
			gameTime.color = Color.red;
		}

		if (time < countdownTime && time > 0) {
			AudioSource.PlayClipAtPoint(countdown, Vector3.zero);

			if (time % 2 == 1){
				GetComponentInParent<Image>().sprite = Resources.Load<Sprite>("MenuTitleBarRed");
				gameTime.color = Color.black;
			}
			else{
				GetComponentInParent<Image>().sprite = Resources.Load<Sprite>("MenuTitleBar");
				gameTime.color = Color.red;
			}
		}
	}

	string DisplayTime()
	{
		int minutes = (int)time / 60;
		int seconds = (int)time % 60;

		return string.Format ("{0:0}", minutes) + ":" + string.Format ("{0:00}", seconds);
	}
}
