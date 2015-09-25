using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	public Transform targetHeavy = null;
	public Transform targetLight = null;

	public Vector3 cameraPosition = Vector3.zero;

	public float damping;
	public float rotationDamping;

	public Vector3 lookOffset = Vector3.zero;


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

		Vector3 wantedPosition = target.TransformPoint (cameraPosition);

		RaycastHit hit;
		if (Physics.Raycast(target.TransformPoint(cameraPosition.x, cameraPosition.y, 0), target.TransformDirection(0, 0, cameraPosition.z), out hit, Mathf.Abs(cameraPosition.z))) {
			wantedPosition = hit.point;
		}

		transform.position = Vector3.Lerp (transform.position, wantedPosition, Time.deltaTime * damping);



		Vector3 lookPosition = target.TransformPoint (lookOffset);

		Quaternion wantedRotation = Quaternion.LookRotation (lookPosition - transform.position, Vector3.up);
		transform.rotation = Quaternion.Slerp (transform.rotation, wantedRotation, Time.deltaTime * rotationDamping);
	}
}
