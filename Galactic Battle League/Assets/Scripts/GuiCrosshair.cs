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
			transform.position = Vector3.Lerp(transform.position, playerCameraHeavy.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position), 10 * Time.deltaTime);
		} else {
			transform.position = Vector3.Lerp(transform.position, playerCameraLight.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position), 10 * Time.deltaTime);
		}
	}
}
