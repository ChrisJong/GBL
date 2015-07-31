using UnityEngine;
using System.Collections;
using InControl;

public class StartGameButton : MonoBehaviour {
	void Update() {
		var inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasReleased) {
			Application.LoadLevel ("ControlsController");
		} else if (inputDevice.MenuWasPressed) {
			Application.Quit();
		}
	}	
}