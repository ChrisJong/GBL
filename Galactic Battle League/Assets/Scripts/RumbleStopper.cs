using UnityEngine;
using System.Collections;
using InControl;

public class RumbleStopper : MonoBehaviour {

	// Use this for initialization
	void Start () {
		foreach (var device in InputManager.Devices) {
			device.Vibrate(0.0f, 0.0f);
		}
	}

	void Update(){
		foreach (var device in InputManager.Devices) {
			device.Vibrate(0.0f, 0.0f);
		}
	}
}
