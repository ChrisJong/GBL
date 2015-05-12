using UnityEngine;
using System;


public class CrosshairControl : MonoBehaviour {

	public GameObject crosshair;

	void Update() {
		RaycastHit hit;

		// crosshair
		if (Physics.Raycast(gameObject.transform.position, 
							gameObject.transform.forward,
							out hit,
							500.0f)) {
			crosshair.transform.position = hit.point;
		} else {
			crosshair.transform.position = gameObject.transform.position + -gameObject.transform.forward * 500.0f;
		}		
	}
}
