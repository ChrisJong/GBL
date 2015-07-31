using UnityEngine;
using System.Collections;
using InControl;

public class StartArenaButton : MonoBehaviour {
	void Update() {
		var inputDevice = InputManager.ActiveDevice;
		if (inputDevice.Action1.WasReleased) {
			GameObject.Destroy(GameObject.Find("MenuMusic"));
			Application.LoadLevel ("arena");
		} else if (inputDevice.MenuWasPressed) {
			Application.Quit();
		}
	}	
}