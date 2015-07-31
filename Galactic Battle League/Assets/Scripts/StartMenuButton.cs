using UnityEngine;
using System.Collections;
using InControl;

public class StartMenuButton : MonoBehaviour {
	void Update() {
		var inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasReleased) {
			Application.LoadLevel ("MainMenuController");
		} else if (inputDevice.MenuWasPressed) {
			Application.Quit();
		}
	}	
}