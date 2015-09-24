using UnityEngine;
using System.Collections;

public class GuiCrosshair : MonoBehaviour {
	public Camera playerCamera;
	public Transform worldCrosshair;
	public int playerNumber;

	// Use this for initialization
	void Start () {
		if (playerCamera == null) {
			playerCamera = GameObject.Find("Camera" + playerNumber).GetComponent<Camera>();
		}
		if (worldCrosshair == null) {
			worldCrosshair = GameObject.Find("Crosshair" + playerNumber).transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, playerCamera.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position), 10 * Time.deltaTime);
	}
}
