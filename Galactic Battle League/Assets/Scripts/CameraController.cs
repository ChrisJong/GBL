using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	private Camera thisCamera;

	public enum CameraMode {Near, Far, FirstPerson};

	public Transform targetHeavy = null;
	public Transform targetLight = null;

	public Vector3 cameraPositionOffset = new Vector3(0, 1.4f, 0);

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
	public float distanceStiffness;

	public Color tankColour;

	float cameraDistanceCurrent;

	public CameraMode cameraMode = CameraMode.Near;
	bool spawnCamera;

	private CameraFilterPack_AAA_SuperHexagon respawnCam;
	private bool respawned = true;
	private CameraFilterPack_AAA_SuperComputer laserCam;
	private bool laserCamActive;
	
	private CameraFilterPack_TV_Artefact glitchCam;
	private float stopGlitch;

	private CameraFilterPack_TV_80 lowHealthCam;

	private CameraFilterPack_FX_Glitch1 signalJammedCam;

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
		respawnCam = GetComponent<CameraFilterPack_AAA_SuperHexagon>();
		respawnCam._BorderColor = tankColour;
		respawnCam._HexaColor = tankColour;
		laserCam = GetComponent<CameraFilterPack_AAA_SuperComputer>();
		laserCam.ChangeRadius = 1.0f;
		laserCam.Radius = 1.0f;
		laserCam._BorderColor = tankColour;
		signalJammedCam = GetComponent<CameraFilterPack_FX_Glitch1>();
		respawnCam.enabled = true;
		respawnCam.ChangeRadius = 0;
		respawnCam.Radius = 0;
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
		if (respawnCam.enabled && !respawned) 
		{
			if (respawnCam.ChangeRadius >= 0)
				respawnCam.ChangeRadius -= 1.0f * Time.deltaTime;
		}
		else if (respawnCam.enabled && respawned) 
		{
			respawnCam.ChangeRadius += 1.0f * Time.deltaTime;
			if (respawnCam.ChangeRadius >= 1.5f)
				respawnCam.enabled = false;
		}

		if (laserCam.enabled) 
		{
			if (laserCam.ChangeRadius >= 0.8f && laserCamActive == true)
				laserCam.ChangeRadius -= 1.75f * Time.deltaTime;

			if (!laserCamActive)
			{
				laserCam.ChangeRadius += 1.75f * Time.deltaTime;
				if (laserCam.ChangeRadius >= 1.0f)
					laserCam.enabled = false;
			}
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
			transform.parent = target.parent;
			transform.position = target.TransformPoint (cameraPositionOffset + cameraPositionSpawn);
			transform.rotation = Quaternion.LookRotation (target.TransformPoint(cameraPositionOffset + lookOffsetSpawn) - transform.position, Vector3.up);
			cameraDistanceCurrent = (transform.position - target.position).magnitude;
		} else if (cameraMode == CameraMode.FirstPerson) {
			transform.parent = target;

			if (target==targetHeavy) {
				transform.position = target.TransformPoint(cameraPositionOffset + cameraPositionFirstPersonHeavy);
			} else {
				transform.position = target.TransformPoint(cameraPositionOffset + cameraPositionFirstPersonLight);
			}
			transform.rotation = target.rotation;
			cameraDistanceCurrent = 0;
		} else {
			transform.parent = target.parent;
			Vector3 wantedPosition = Vector3.zero;
			Vector3 lookPosition = Vector3.zero;

			if (cameraMode == CameraMode.Near) {
				wantedPosition = target.TransformPoint (cameraPositionOffset + (cameraPositionNear * cameraDistanceCurrent / cameraDistanceNear));

				RaycastHit hit;
				if (Physics.Raycast(target.TransformPoint(cameraPositionOffset) , target.TransformDirection(cameraPositionNear), out hit, cameraDistanceCurrent)) {
					wantedPosition = hit.point;
					cameraDistanceCurrent = hit.distance;
				} else {
					cameraDistanceCurrent = Mathf.Lerp(cameraDistanceCurrent, cameraDistanceNear, Time.deltaTime * distanceStiffness);
				}
			
				lookPosition = target.TransformPoint (cameraPositionOffset + lookOffsetNear);
			} else if (cameraMode == CameraMode.Far) {
				wantedPosition = target.TransformPoint (cameraPositionOffset + (cameraPositionFar * cameraDistanceCurrent / cameraDistanceFar));
				
				RaycastHit hit;
				if (Physics.Raycast(target.TransformPoint(cameraPositionOffset), target.TransformDirection(cameraPositionFar), out hit, cameraDistanceCurrent)) {
					wantedPosition = hit.point;
					cameraDistanceCurrent = hit.distance;
				} else {
					cameraDistanceCurrent = Mathf.Lerp(cameraDistanceCurrent, cameraDistanceFar, Time.deltaTime * distanceStiffness);
				}

				lookPosition = target.TransformPoint (cameraPositionOffset + lookOffsetFar);				
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

	public void RunDeath()
	{
		if (respawned) 
		{
			respawnCam.ChangeRadius = 1.0f;
			respawnCam.enabled = true;
			respawned = false;
		}
	}

	public void RunRespawn()
	{
		respawnCam.ChangeRadius = 0;
		respawnCam.enabled = true;
		spawnCamera = true;
		respawned = true;
	}

	public void RunLowHealth()
	{
		lowHealthCam.enabled = true;
	}

	public void StopLowHealth()
	{
		lowHealthCam.enabled = false;
	}

	public void RunSignalJammed()
	{
		signalJammedCam.enabled = true;
		print("true");
	}

	public void StopSignalJammed()
	{
		signalJammedCam.enabled = false;
		print ("false");
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

	public void RunLaser()
	{
		laserCam.enabled = true;
		laserCamActive = true;
	}

	public void StopLaser()
	{
		laserCamActive = false;

	}
}
