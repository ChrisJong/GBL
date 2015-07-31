using UnityEngine;
using System.Collections;
using InControl;

public class EscapeCombo : MonoBehaviour {
	void Update () {
	var inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action3.IsPressed && inputDevice.Action4.IsPressed && inputDevice.LeftBumper.IsPressed && inputDevice.RightBumper.IsPressed) {
			Application.LoadLevel ("MainMenuController");
		}
	}
}
