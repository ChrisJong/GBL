﻿using UnityEngine;
using System;
using InControl;

public class AimControl : MonoBehaviour {

	public int playerNumber;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;

		// up/down aiming
		if (inputDevice != null) {
			if (Math.Abs(inputDevice.RightStickY) > 0.20 * Math.Abs(inputDevice.RightStickY))
				gameObject.transform.Rotate(Vector3.left, inputDevice.RightStickY);
		}

	}
}
