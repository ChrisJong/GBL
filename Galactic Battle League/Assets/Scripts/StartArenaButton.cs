using UnityEngine;
using System.Collections;
using InControl;

public class StartArenaButton : MonoBehaviour {
	void Update() {
		var inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasPressed) {
			Application.LoadLevel ("arena");
		} else if (inputDevice.MenuWasPressed) {
			Application.Quit();
		}
	}	
}