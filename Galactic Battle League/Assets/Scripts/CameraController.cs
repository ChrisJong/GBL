using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Camera thisCamera;

	public enum CameraMode {Near, Far, FirstPerson};

	public Transform targetHeavy = null;
	public Transform targetLight = null;

	public Vector3 cameraPositionNear = Vector3.zero;
	float cameraDistanceNear;
	public Vector3 lookOffsetNear = Vector3.zero;

	public Vector3 cameraPositionFar = Vector3.zero;
	float cameraDistanceFar;
	public Vector3 lookOffsetFar = Vector3.zero;

	public Vector3 cameraPositionSpawn = Vector3.zero;
	float cameraDistanceSpawn;
	public Vector3 lookOffsetSpawn = Vector3.zero;

	public Vector3 cameraPositionFirstPersonLight = Vector3.zero;
	public Vector3 cameraPositionFirstPersonHeavy = Vector3.zero;

	public float stiffness;
	public float rotationStiffness;
	public CameraMode cameraMode = CameraMode.Near;
	bool spawnCamera;

	private CameraFilterPack_AAA_SuperComputer respawnCam;
	
	private CameraFilterPack_TV_Artefact glitchCam;
	private float stopGlitch;

	private CameraFilterPack_TV_80 lowHealthCam;

	private CameraFilterPack_FX_EarthQuake quakeCam;
	private float stopQuake;

	private CameraFilterPack_Drawing_Manga_FlashWhite dashCam;
	private float stopDash;

	private CameraFilterPack_Distortion_ShockWave shockwaveCam;
	private float stopShockwave;
	//NOTE TO SELF for BROKEN GLASS 2: do not use bullets 2, 3, or 6
	// Use this for initialization
	void Start () {
		thisCamera = GetComponent<Camera> ();
		glitchCam = GetComponent<CameraFilterPack_TV_Artefact>();
		respawnCam = GetComponent<CameraFilterPack_AAA_SuperComputer>();
		respawnCam.enabled = true;
		respawnCam.ChangeRadius = 0;
		lowHealthCam = GetComponent<CameraFilterPack_TV_80> ();
		quakeCam = GetComponent<CameraFilterPack_FX_EarthQuake> ();
		dashCam = GetComponent<CameraFilterPack_Drawing_Manga_FlashWhite> ();
		dashCam.Speed = 10;
		shockwaveCam = GetComponent<CameraFilterPack_Distortion_ShockWave> ();
		shockwaveCam.Speed = 2f;

		cameraDistanceNear = cameraPositionNear.magnitude;
		cameraDistanceFar = cameraPositionFar.magnitude;
		spawnCamera = true;
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

		if (quakeCam.enabled == true && stopQuake < Time.time)
			quakeCam.enabled = false;

		if (dashCam.enabled == true && stopDash < Time.time)
			dashCam.enabled = false;
		
		if (shockwaveCam.enabled == true && stopShockwave < Time.time)
			shockwaveCam.enabled = false;

		Transform target;
		if (targetHeavy.gameObject.activeInHierarchy) {
			target = targetHeavy;
		} else {
			target = targetLight;
		}

		if (spawnCamera) {
			transform.position = target.TransformPoint (cameraPositionSpawn);
			transform.rotation = Quaternion.LookRotation (target.TransformPoint(lookOffsetSpawn) - transform.position, Vector3.up);
		} else if (cameraMode == CameraMode.FirstPerson) {
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
				if (Physics.Raycast(target.TransformPoint(cameraPositionNear.x, cameraPositionNear.y, 0), target.TransformDirection(0, 0, cameraPositionNear.z), out hit, cameraDistanceNear)) {
					wantedPosition = hit.point;
				}
			
				lookPosition = target.TransformPoint (lookOffsetNear);
			} else if (cameraMode == CameraMode.Far) {
				wantedPosition = target.TransformPoint (cameraPositionFar);
				
				RaycastHit hit;
				if (Physics.Raycast(target.TransformPoint(cameraPositionFar.x, cameraPositionFar.y, 0), target.TransformDirection(0, 0, cameraPositionFar.z), out hit, cameraDistanceFar)) {
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
		if (spawnCamera) {
			spawnCamera = false;
		} else {
			if (cameraMode == CameraMode.Near)
				cameraMode = CameraMode.Far;
			else if (cameraMode == CameraMode.Far)
				cameraMode = CameraMode.FirstPerson;
			else 
				cameraMode = CameraMode.Near;
		}
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
		spawnCamera = true;
	}

	public void RunLowHealth()
	{
		lowHealthCam.enabled = true;
	}

	public void StopLowHealth()
	{
		lowHealthCam.enabled = false;
	}

	public void RunQuake(float intensity)
	{
		if (quakeCam.enabled == true)
			quakeCam.enabled = false;
		quakeCam.X = intensity;
		quakeCam.Y = intensity;
		stopQuake = Time.time + 0.5f;
		quakeCam.enabled = true;
	}

	public void RunDash()
	{
		dashCam.enabled = true;
		stopDash = Time.time + 0.5f;
	}

	public void RunShockwave(Vector3 screenPoint)
	{
		shockwaveCam.TimeX = 1.0f;
		shockwaveCam.PosY = 0.45f;
		//shockwaveCam.PosX = thisCamera.WorldToScreenPoint (screenPoint).x;
		//shockwaveCam.PosY = thisCamera.WorldToScreenPoint (screenPoint).y;
		shockwaveCam.enabled = true;
		stopShockwave = Time.time + 0.9f/shockwaveCam.Speed;
	}
}
