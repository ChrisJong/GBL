﻿using UnityEngine;
using System.Collections;
using InControl;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
	Rigidbody m_body;
	private bool initialised = false;
	float m_deadZone = 0.1f;
	
	public float m_hoverForce = 9.0f;
	public float m_hoverHeight = 2.0f;
	public GameObject[] m_hoverPoints;
	
	public float m_forwardAcl = 100.0f;
	public float m_backwardAcl = 25.0f;
	float m_currThrust = 0.0f;
	
	public float m_sideAcl = 25.0f;
	float m_currSideThrust = 0.0f;
	
	public float m_turnStrength = 10f;
	float m_currTurn = 0.0f;
	
	public GameObject m_leftAirBrake;
	public GameObject m_rightAirBrake;
	
	public float maxAbilityCharge;
	private float abilityCharge;
	public float abilityChargeRate;
	public float abilityUseRate;
	
	public int playerNumber; 
	public int tankClass;
	
	int m_layerMask;
	
	public GameObject sparkParticle;
	
	public ParticleSystem damage33;
	public ParticleSystem damage66;
	public ParticleSystem[] hoverParticles;
	public ParticleSystem baseParticle;
	private float particleLength;
	
	//Death/respawn variables
	public UIController uiController;
	public HealthCounter health;
	public AudioClip sfxDeath;
	Vector3 initialPosition;
	Quaternion initialRotation;
	public ParticleSystem hitParticle;
	private double spawnActiveTimer;
	
	public GameObject respawnMessage1;
	public GameObject respawnMessage2;
	
	private float tempHoverForce;
	private float timer = 0.0f;
	private bool deathRun = false;
	public int maxHealth = 100;
	private int healthInt;
	
	public bool hasRespawned = true;
	
	public Animator[] spawnAnimators;
	
	//Fire control variables
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public AudioClip sfxFire;
	public Vector3 tankVelocity;
	private float nextFire;
	public AudioClip sfxHit;
	public float explosionRadius = 4.0F;
	public float explosionPower = 25000.0F;
	public ParticleSystem fireParticle;
	private float maxError = 6.0f;
	private float currError = 0.0f;
	
	private float rumbleTime;
	
	public AudioClip killCheer = null;
	
	void Start()
	{
		foreach (Animator anim in spawnAnimators) 
		{
			anim.enabled = false;
		}
		
		healthInt = maxHealth;
		abilityCharge = maxAbilityCharge;

		initialPosition = gameObject.transform.position;
		initialRotation = gameObject.transform.rotation;
		
		m_body = GetComponent<Rigidbody>();
		
		m_layerMask = 1 << LayerMask.NameToLayer("Characters");
		m_layerMask = ~m_layerMask;
		
		respawnMessage1.SetActive(true);
		respawnMessage2.SetActive(true);
		
		initialised = true;
		particleLength = hoverParticles[0].startLifetime;
	}
	
	void OnDrawGizmos()
	{
		//  Hover Force
		RaycastHit hit;
		for (int i = 0; i < m_hoverPoints.Length; i++)
		{
			var hoverPoint = m_hoverPoints [i];
			if (Physics.Raycast(hoverPoint.transform.position, 
			                    -Vector3.up, out hit,
			                    m_hoverHeight, 
			                    m_layerMask))
			{
				Gizmos.color = Color.blue;
				Gizmos.DrawLine(hoverPoint.transform.position, hit.point);
				Gizmos.DrawSphere(hit.point, 0.5f);
			} 
			else
			{
				Gizmos.color = Color.red;
				Gizmos.DrawLine(hoverPoint.transform.position, 
				                hoverPoint.transform.position - Vector3.up * m_hoverHeight);
			}
		}
	}
	
	void Update()
	{
		var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;
		health.healthscore = healthInt;
		if (hasRespawned && inputDevice != null) 
		{
			if (inputDevice.RightTrigger.IsPressed) 
			{
				foreach (Animator anim in spawnAnimators)
				{
					anim.enabled = true;
					anim.Play(0, -1, 0f);
				}
				
				respawnMessage1.SetActive(false);
				respawnMessage2.SetActive(false);
				spawnActiveTimer = Time.time + 1.0f;
				hasRespawned = false;
			}
		}
		else if (!deathRun && inputDevice != null && spawnActiveTimer < Time.time) 
		{
			foreach(ParticleSystem particle in hoverParticles)
			{
				particle.startLifetime = particleLength;
			}
			
			// Main Thrust
			m_currThrust = 0.0f;
			float aclAxis = inputDevice.Direction.Y;
			if (aclAxis > m_deadZone)
			{
				foreach(ParticleSystem particle in hoverParticles)
				{
					particle.startLifetime = particleLength*2;
				}
				m_currThrust = aclAxis * m_forwardAcl;
			}
			else if (aclAxis < -m_deadZone)
			{
				foreach(ParticleSystem particle in hoverParticles)
				{
					particle.startLifetime = particleLength*0.5f;
				}
				m_currThrust = aclAxis * m_backwardAcl;
			}
			
			// Side Thrust
			m_currSideThrust = 0.0f;
			float aclSideAxis = inputDevice.Direction.X;
			if (aclSideAxis > m_deadZone)
				m_currSideThrust = aclSideAxis * m_sideAcl;
			else if (aclSideAxis < -m_deadZone)
				m_currSideThrust = aclSideAxis * m_sideAcl;
			
			// Turning
			m_currTurn = 0.0f;
			float turnAxis = inputDevice.RightStickX * inputDevice.RightStickX * inputDevice.RightStickX;
			if (Mathf.Abs (turnAxis) > m_deadZone)
				m_currTurn = turnAxis;
			
			
			// Firing
			if (inputDevice.RightTrigger.IsPressed && Time.time > nextFire) 
			{
				nextFire = Time.time + fireRate;
				Rumble(0.15f);
				gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, shotSpawn.position, explosionRadius);
				tankVelocity = GetComponent<Rigidbody>().velocity;
				fireParticle.Play();
				createShot (tankVelocity);
				AudioSource.PlayClipAtPoint (sfxFire, shotSpawn.position, 0.5f);
			}
			
			//Ability
			if (inputDevice.LeftTrigger.IsPressed && abilityCharge > 0f)
			{
				abilityCharge -= abilityUseRate * Time.deltaTime;
				
				if (tankClass == 1) {
					//Boost ability
					m_currThrust = 25.0f * aclAxis * m_forwardAcl;
					m_currSideThrust = 25.0f * aclSideAxis * m_sideAcl;

					foreach(ParticleSystem particle in hoverParticles)
					{
						particle.startLifetime = particleLength*4;
					}
				} else {
					//Laser ability
					RaycastHit hit;
					bool hitWall = false;
					Physics.Raycast(shotSpawn.position, shotSpawn.forward, out hit, 300);
					if (hit.collider.tag != "Player"){
						hitWall = true;
					}

					/*while (!hitWall){
						Physics.Raycast(hit.point, shotSpawn.forward, out hit, 300);
						Physics.IgnoreCollision(hit.collider, collider); 
						if (hit.collider.tag != "Player"){
							hitWall = true;
						}
					}*/

					Debug.DrawLine (shotSpawn.position, hit.point, Color.cyan);
					//print(hitscan.collider.name);
				}

				//Shield ability

				

			}
		}

		if (currError > 0 && !inputDevice.RightTrigger.IsPressed) 
			currError -= 1.0f * Time.deltaTime;

		if (abilityCharge < maxAbilityCharge && !inputDevice.LeftTrigger.IsPressed){
			abilityCharge += abilityChargeRate * Time.deltaTime;

		}

		if (gameObject.transform.position.y <= -100) 
		{
			Death ();
			Respawn ();
		}
		
		if (healthInt <= 0) 
		{
			if (!deathRun)
				Death ();
			timer += Time.deltaTime;
			if(timer > 5.0f)
			{
				Respawn();
			}
		}
	}
	
	void FixedUpdate()
	{
		
		//  Hover Force
		RaycastHit hit;
		for (int i = 0; i < m_hoverPoints.Length; i++)
		{
			var hoverPoint = m_hoverPoints [i];
			if (Physics.Raycast(hoverPoint.transform.position, 
			                    -Vector3.up, out hit,
			                    m_hoverHeight,
			                    m_layerMask))
				m_body.AddForceAtPosition(Vector3.up 
				                          * m_hoverForce
				                          * (1.0f - (hit.distance / m_hoverHeight)), 
				                          hoverPoint.transform.position);
			else
			{
				if (transform.position.y > hoverPoint.transform.position.y)
					m_body.AddForceAtPosition(
						hoverPoint.transform.up * m_hoverForce,
						hoverPoint.transform.position);
				else
					m_body.AddForceAtPosition(
						hoverPoint.transform.up * -m_hoverForce,
						hoverPoint.transform.position);
			}
		}
		
		// Forward
		if (Mathf.Abs(m_currThrust) > 0)
			m_body.AddForce(transform.forward * m_currThrust);
		
		
		// Sideways
		if (Mathf.Abs(m_currSideThrust) > 0)
			m_body.AddForce(transform.right * m_currSideThrust);
		
		// Turn
		if (m_currTurn > 0)
		{
			m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
		} else if (m_currTurn < 0)
		{
			m_body.AddRelativeTorque(Vector3.up * m_currTurn * m_turnStrength);
		}
		
		// Rumble
		var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;
		if (inputDevice !=null && Time.time < rumbleTime)
		{
			inputDevice.Vibrate(1.0f, 1.0f);
		} else if (inputDevice != null) {
			inputDevice.Vibrate(0.0f, 0.0f);
		}
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (deathRun == true)
			return;
		if (other.tag == "Shot")
		{
			ShotController shotControllerCopy = other.gameObject.GetComponent<ShotController>();
			if (shotControllerCopy.playerNumber != playerNumber)
			{
				AudioSource.PlayClipAtPoint(sfxHit, gameObject.transform.position, 0.25f);
				Vector3 explosionPos = other.gameObject.transform.position;
				Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
				hitParticle.transform.position = other.transform.position;
				hitParticle.Play();
				foreach (Collider hit in colliders) 
				{
					// if (hit && hit.GetComponent<Rigidbody>())
					// hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionRadius);
					
				}
				healthInt -= shotControllerCopy.damage;
				Destroy (other.gameObject);				
				Rumble(0.15f);
				if ((float)healthInt/(float)maxHealth < .66f)
					if (damage66)
						damage66.Play();
				if ((float)healthInt/(float)maxHealth < .33f)
					if (damage33)
						damage33.Play();
				if (healthInt <= 0)
				{
					healthInt = 0;
					uiController.PlayerKill(shotControllerCopy.playerNumber, playerNumber);
				}
			}
		}
	}
	
	void OnCollisionEnter(Collision collision)
	{
		foreach (ContactPoint contact in collision.contacts) 
		{
			if (contact.otherCollider.tag != "Shot" + playerNumber)
			{
				ParticleEmitter sparks = ((GameObject)Instantiate(sparkParticle, contact.point, shotSpawn.rotation)).GetComponent<ParticleEmitter>();
				sparks.Emit();
				Rumble(0.1f);
			}
		}
	}
	
	void createShot(Vector3 tankVelocity)
	{
		Quaternion shotAngle = shotSpawn.rotation;
		if (tankClass == 1) 
		{
			Vector2 error = Random.insideUnitCircle * currError;
			Quaternion errorRotation = Quaternion.Euler(error.x/2, error.y, 0);
			shotAngle = shotAngle * errorRotation;
			if (currError < maxError)
				currError += 0.1f;
		}
		GameObject zBullet = (GameObject)Instantiate (shot, shotSpawn.position, shotAngle);
		zBullet.GetComponent<ShotController> ().SetVelocity ();
	}
	
	void Death()
	{
		tempHoverForce = m_hoverForce;
		m_hoverForce = 0.0f;
		
		m_currThrust = 0.0f;
		m_currSideThrust = 0.0f;
		m_currTurn = 0.0f;
		
		AudioSource.PlayClipAtPoint(sfxDeath, gameObject.transform.position, 0.25f);
		if (killCheer) {
			AudioSource.PlayClipAtPoint(killCheer, gameObject.transform.position, 0.25f);
		}
		timer = 0.0f;
		deathRun = true;
		foreach (ParticleSystem particle in hoverParticles) 
		{
			particle.Stop();
		}
		baseParticle.Stop ();
	}
	
	void Respawn()
	{
		if (damage33)
			damage33.Stop ();
		if (damage66)
			damage66.Stop ();
		gameObject.transform.position = initialPosition;
		gameObject.transform.rotation = initialRotation;
		m_body.velocity = Vector3.zero;
		m_body.angularVelocity = Vector3.zero;
		m_hoverForce = tempHoverForce;
		foreach (ParticleSystem particle in hoverParticles) 
		{
			if (particle)
				particle.Play();
		}
		if (baseParticle)
			baseParticle.Play ();
		healthInt = maxHealth;
		deathRun = false;
		hasRespawned = true;
		
		respawnMessage1.SetActive(true);
		respawnMessage2.SetActive(true);
	}
	
	void Rumble(float duration) {
		if (rumbleTime < Time.time + duration)
			rumbleTime = Time.time + duration;
	}
	
	void OnEnable()
	{
		if (initialised) {
			gameObject.transform.position = initialPosition;
			gameObject.transform.rotation = initialRotation;
			healthInt = maxHealth;
		}
	}
	void OnDisable()
	{
		var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;
		rumbleTime = 0.0f;
		if (inputDevice != null) {
			inputDevice.Vibrate(0.0f, 0.0f);
		}
	}
}
