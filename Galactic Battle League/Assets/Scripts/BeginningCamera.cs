using UnityEngine;
using System.Collections;
using InControl;

public class BeginningCamera : MonoBehaviour {
	public Material fadeBlack; //shader material for quad
	public float fadeSpeed; //speed of fade in/out
	public GameObject UIRoot;
	public GameObject Player1;
	public GameObject Player2;
	public GameObject Player3;
	public GameObject Player4;
	public float TimeUntilSkippable;
	int fadeWay; //1=Out to black -1=In from black
	Color fadeControl; //temporary variable to control material's colour
	bool skipped = false; //determines whether this cut scene has skipped or not
	float timer; //temporary variable to store time elapsed during cut scene

	void Update (){
		//controls fading either in or out
		fadeBlack.color = fadeControl;
		if (fadeControl.a >= 0.0f && fadeControl.a <= 1.0f) {
			fadeControl.a = (fadeControl.a + (fadeSpeed * fadeWay));
			Debug.Log (fadeControl.a);
		}
		//allows skipping after an amount of time
		if (timer >= TimeUntilSkippable && skipped == false) {
			if(InputManager.ActiveDevice.AnyButton.WasPressed) {
				skipped = true;
				FadeOut ();
			}
		}
		//only to start game once faded to black
		if (skipped == true) {
			if(fadeControl.a >= 1.0f){
				StartGame();
			}
		}
		//counts time elapsed
		timer += Time.deltaTime;
	}

	void FadeIn (){
		fadeWay = -1;
		fadeControl.a = 1.0f;
	}

	void FadeOut (){
		fadeWay = 1;
		if (fadeControl.a < 0) {
			fadeControl.a = 0.0f;
		}
	}

	void StartGame (){
		UIRoot.SetActive (true);
		Player1.SetActive (true);
		Player2.SetActive (true);
		Player3.SetActive (true);
		Player4.SetActive (true);
		Destroy (gameObject);
	}
}