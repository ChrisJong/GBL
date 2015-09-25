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


	// Use this for initialization
	void Start () {
	
	}
	
	void FixedUpdate () {
		Transform target;
		if (targetHeavy.gameObject.activeInHierarchy) {
			target = targetHeavy;
		} else {
			target = targetLight;
		}

		if (cameraMode == CameraMode.FirstPerson) {
			if (target==targetHeavy) {
				transform.position = target.TransformPoint(cameraPositionFirstPersonHeavy);
			} else {
				transform.position = target.TransformPoint(cameraPositionFirstPersonLight);
			}
			transform.rotation = target.rotation;
		} else {
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
}
