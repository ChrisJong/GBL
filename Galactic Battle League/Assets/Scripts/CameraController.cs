using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public enum CameraMode {Near, Far, FirstPerson};
	
	public Transform targetHeavy = null;
	public Transform targetLight = null;

	public Vector3 cameraPositionNear = Vector3.zero;
	public Vector3 lookOffsetNear = Vector3.zero;

	public Vector3 cameraPositionFar = Vector3.zero;
	public Vector3 lookOffsetFar = Vector3.zero;

	public Vector3 cameraPositionFirstPersonLight = Vector3.zero;
	public Vector3 cameraPositionFirstPersonHeavy = Vector3.zero;

	public float stiffness;
	public float rotationStiffness;
	public CameraMode cameraMode = CameraMode.Near;

	private CameraFilterPack_AAA_SuperComputer respawnCam;
	
	private CameraFilterPack_TV_Artefact glitchCam;
	private float stopGlitch;

	private CameraFilterPack_TV_80 lowHealthCam;

	// Use this for initialization
	void Start () {
		glitchCam = GetComponent<CameraFilterPack_TV_Artefact>();
		respawnCam = GetComponent<CameraFilterPack_AAA_SuperComputer>();
		respawnCam.enabled = true;
		respawnCam.ChangeRadius = 0;
		lowHealthCam = GetComponent<CameraFilterPack_TV_80> ();
	}

	void Update()
	{
		if (respawnCam.enabled == true) 
		{
			respawnCam.ChangeRadius += 0.03f;
			if (respawnCam.ChangeRadius >= 1.5f)
				respawnCam.enabled = false;
		}
	}

	void FixedUpdate () {
		if (glitchCam.enabled == true && stopGlitch < Time.time)
			glitchCam.enabled = false;

		Transform target;
		if (targetHeavy.gameObject.activeInHierarchy) {
			target = targetHeavy;
		} else {
			target = targetLight;
		}

		if (cameraMode == CameraMode.FirstPerson) {
			transform.parent = target;

			if (target==targetHeavy) {
				transform.position = target.TransformPoint(cameraPositionFirstPersonHeavy);
			} else {
				transform.position = target.TransformPoint(cameraPositionFirstPersonLight);
			}
			transform.rotation = target.rotation;
		} else {
			transform.parent = null;
			Vector3 wantedPosition = Vector3.zero;
			Vector3 lookPosition = Vector3.zero;

			if (cameraMode == CameraMode.Near) {
				wantedPosition = target.TransformPoint (cameraPositionNear);
				
				RaycastHit hit;
				if (Physics.Raycast(target.TransformPoint(cameraPositionNear.x, cameraPositionNear.y, 0), target.TransformDirection(0, 0, cameraPositionNear.z), out hit, Mathf.Abs(cameraPositionNear.z))) {
					wantedPosition = hit.point;
				}
			
				lookPosition = target.TransformPoint (lookOffsetNear);
			} else if (cameraMode == CameraMode.Far) {
				wantedPosition = target.TransformPoint (cameraPositionFar);
				
				RaycastHit hit;
				if (Physics.Raycast(target.TransformPoint(cameraPositionFar.x, cameraPositionFar.y, 0), target.TransformDirection(0, 0, cameraPositionFar.z), out hit, Mathf.Abs(cameraPositionFar.z))) {
					wantedPosition = hit.point;
				}

				lookPosition = target.TransformPoint (lookOffsetFar);				
			}

			transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * stiffness);
			Quaternion wantedRotation = Quaternion.LookRotation (lookPosition - transform.position, Vector3.up);
			transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationStiffness);
		}
	}

	public void ChangeMode() {
		if (cameraMode == CameraMode.Near)
			cameraMode = CameraMode.Far;
		else if (cameraMode == CameraMode.Far)
			cameraMode = CameraMode.FirstPerson;
		else 
			cameraMode = CameraMode.Near;
	}

	public void RunGlitch()
	{
		glitchCam.enabled = true;
		stopGlitch = Time.time + 0.5f;
	}
	
	public void RunRespawn()
	{
		respawnCam.ChangeRadius = 0;
		respawnCam.enabled = true;
	}

	public void RunLowHealth()
	{
		lowHealthCam.enabled = true;
	}

	public void StopLowHealth()
	{
		lowHealthCam.enabled = false;
	}
}
