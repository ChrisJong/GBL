using UnityEngine;
using System.Collections;

public class GuiCrosshair : MonoBehaviour {
	public GameObject playerCameraHeavy;
	public GameObject playerCameraLight;
	public Transform worldCrosshair;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (playerCameraHeavy.activeInHierarchy) {
			transform.position = playerCameraHeavy.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position);
		} else {
			transform.position = playerCameraLight.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position);
		}
	}
}
