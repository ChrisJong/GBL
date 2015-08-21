using UnityEngine;
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

	public float m_aimStrength = 10f;
	float m_currAim = 0.0f;
	
	public GameObject m_leftAirBrake;
	public GameObject m_rightAirBrake;
	
	private float nextAbility;
	public float abilityRate;
	
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
	public GameObject hitParticle;
	public ParticleSystem[] deathParticle;
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
	public Transform[] shotSpawn;
	public ParticleSystem[] fireParticle;
	//Int declares which cannon to use
	private int spawnInt;
	public float fireRate;
	public AudioClip sfxFire;
	public Vector3 tankVelocity;
	private float nextFire;
	public AudioClip sfxHit;
	public float explosionRadius = 4.0F;
	public float explosionPower = 25000.0F;
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
		
		initialPosition = gameObject.transform.position;
		initialRotation = gameObject.transform.rotation;
		
		m_body = GetComponent<Rigidbody>();
		
		m_layerMask = 1 << LayerMask.NameToLayer("Characters");
		m_layerMask = ~m_layerMask;
		
		respawnMessage1.SetActive(true);
		respawnMessage2.SetActive(true);
		
		initialised = true;
		particleLength = hoverParticles[0].startLifetime;
		spawnInt = 0;
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

			// up/down aiming
			m_currAim = 0.0f;
			float aimAxis = inputDevice.RightStickY * inputDevice.RightStickY * inputDevice.RightStickY;
			if (Mathf.Abs(aimAxis) > m_deadZone){
			    m_currAim = aimAxis;
			}
			
			
			// Firing
			if (inputDevice.RightTrigger.IsPressed && Time.time > nextFire) 
			{
				nextFire = Time.time + fireRate;
				if (tankClass == 1)
					Rumble(0.15f);
				else
					Rumble (0.3f);
				gameObject.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, shotSpawn[spawnInt].position, explosionRadius);
				tankVelocity = GetComponent<Rigidbody>().velocity;
				fireParticle[spawnInt].Play();
				createShot (tankVelocity);
				AudioSource.PlayClipAtPoint (sfxFire, shotSpawn[spawnInt].position, 0.5f);
				spawnInt++;
				if (spawnInt >= shotSpawn.Length)
				{
					spawnInt = 0;
				}
			}
			
			//Ability
			/*if (inputDevice.LeftTrigger.IsPressed && Time.time > nextAbility)
			{
				//nextAbility = Time.time + abilityRate;
				
				//Laser ability

				RaycastHit hitscan;
				Physics.Raycast(shotSpawn.position, shotSpawn.forward, out hitscan, 300);
				Debug.DrawLine (shotSpawn.position, hitscan.point, Color.cyan);
				//print(hitscan.collider.name);

				//Shield ability

				

				
				//Boost ability

				m_currThrust = 25.0f * aclAxis * m_forwardAcl;
				m_currSideThrust = 25.0f * aclSideAxis * m_sideAcl;

				foreach(ParticleSystem particle in hoverParticles)
				{
					particle.startLifetime = particleLength*4;
				}

			}*/
		}

		if (currError > 0 && !inputDevice.RightTrigger.IsPressed) 
			currError -= 1.0f * Time.deltaTime;

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

		// up down aiming

		if (m_currAim > 0) {
			m_body.AddTorque (-transform.right * m_currAim * m_aimStrength);
		} else if (m_currAim < 0) {
			m_body.AddTorque (-transform.right * m_currAim * m_turnStrength / 2);
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
				//Vector3 explosionPos = other.gameObject.transform.position;
				//Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
				ParticleSystem hitExplosion = ((GameObject)Instantiate(hitParticle, other.transform.position, shotSpawn[0].rotation)).GetComponent<ParticleSystem>();
				//hitExplosion.startLifetime = (float)shotControllerCopy.damage/10.0f;
				if (shotControllerCopy.damage >= 10 )
					hitExplosion.startSize = 6;
				else
					hitExplosion.startSize = 3;
				
				hitExplosion.Play();
				//foreach (Collider hit in colliders) 
				//{
					// if (hit && hit.GetComponent<Rigidbody>())
					// hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionRadius);
					
				//}
				healthInt -= shotControllerCopy.damage;

				//Destroy other shot and smoke trail properly
				ParticleSystem smoke = other.GetComponentInChildren<ParticleSystem> ();
				smoke.enableEmission = false;
				smoke.transform.parent=null;
				Destroy(smoke, 3);
				Destroy (other.gameObject);

				if (shotControllerCopy.damage >= 10 )
					Rumble(0.3f);
				else
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
				ParticleEmitter sparks = ((GameObject)Instantiate(sparkParticle, contact.point, shotSpawn[0].rotation)).GetComponent<ParticleEmitter>();
				sparks.Emit();
				Rumble(0.1f);
			}
		}
	}
	
	void createShot(Vector3 tankVelocity)
	{
		Quaternion shotAngle = shotSpawn[spawnInt].rotation;
		if (tankClass == 1) 
		{
			Vector2 error = Random.insideUnitCircle * currError;
			Quaternion errorRotation = Quaternion.Euler(error.x/2, error.y, 0);
			shotAngle = shotAngle * errorRotation;
			if (currError < maxError)
				currError += 0.1f;
		}
		GameObject zBullet = (GameObject)Instantiate (shot, shotSpawn[spawnInt].position, shotAngle);
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
		Vector3 point = gameObject.transform.position;
		point.y += 4;

		foreach (ParticleSystem deathExplosion in deathParticle)
			deathExplosion.Play();

		foreach (ParticleSystem particle in hoverParticles) 
		{
			particle.Stop();
		}
		baseParticle.Stop ();
	}
	
	void Respawn()
	{
		if (damage33) 
		{
			damage33.Stop ();
			damage33.Clear ();
		}
		if (damage66) 
		{
			damage66.Stop ();
			damage66.Clear ();
		}
		foreach (ParticleSystem deathExplosion in deathParticle) 
		{
			deathExplosion.Stop ();
			deathExplosion.Clear();
		}

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
