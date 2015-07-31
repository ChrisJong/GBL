using UnityEngine;
using System.Collections;
using InControl;


public class VehicleChoiceControl : MonoBehaviour {
	
	public GameObject lightVehicle;
	public HoverCarControl lightControl;
	public GameObject heavyVehicle;
	public HoverCarControl heavyControl;
	public float spawnHeight;
	public int playerNumber;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.GetInt("Player" + playerNumber + "Tank") == 1) {
			heavyVehicle.SetActive(false);
		} else {
			lightVehicle.SetActive(false);
		}

	}
	
	// Update is called once per frame
	void Update () {
		if (lightControl.hasRespawned && heavyControl.hasRespawned) {
			var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;

			if (inputDevice != null) {
				if (inputDevice.LeftBumper.WasPressed) {
					PlayerPrefs.SetInt("Player" + playerNumber + "Tank", 1);
					heavyVehicle.SetActive(false);
					lightVehicle.SetActive(true);
				}
				if (inputDevice.RightBumper.WasPressed) {
					PlayerPrefs.SetInt("Player" + playerNumber + "Tank", 2);
					lightVehicle.SetActive(false);
					heavyVehicle.SetActive(true);
				}
			}
		}
	}
}
