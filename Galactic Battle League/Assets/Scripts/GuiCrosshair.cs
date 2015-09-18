using UnityEngine;
using System.Collections;

public class GuiCrosshair : MonoBehaviour {
	public Camera playerCamera;
	public Transform worldCrosshair;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp(transform.position, playerCamera.GetComponent<Camera>().WorldToScreenPoint(worldCrosshair.position), 10 * Time.deltaTime);
	}
}
