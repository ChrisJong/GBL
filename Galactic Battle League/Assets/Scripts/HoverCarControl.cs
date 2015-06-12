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

  	public GameObject m_leftAirBrake;
	public GameObject m_rightAirBrake;

	public int playerNumber; 
	public int tankClass;

  	int m_layerMask;

	public GameObject sparkParticle;

	public ParticleSystem[] hoverParticles;

	//Death/respawn variables
	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;
	public HealthCounter health;
	public GameObject killedMessage;
	public GameObject killMessage1;
	public GameObject killMessage2;
	public GameObject killMessage3;
	public GameObject killMessage4;
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
	private float rumbleTime;

	public AudioClip killCheer = null;
	public bool isAI = false;
	public AIController aiController;

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
		if (isAI) // AI control
		{
			health.healthscore = healthInt;

			if (hasRespawned) 
			{
				foreach (Animator anim in spawnAnimators)
				{
					anim.enabled = true;
					anim.Play(anim.GetCurrentAnimatorStateInfo(0).nameHash,-1,0f);
				}
				
				respawnMessage1.SetActive(false);
				respawnMessage2.SetActive(false);
				spawnActiveTimer = Time.time + 1.0f;
				hasRespawned = false;
			}

			else if (!deathRun && spawnActiveTimer < Time.time) 
			{
				aiController.AIUpdate(playerNumber);
				// Main Thrust
				m_currThrust = 0.0f;
				float aclAxis = aiController.moveY;
				if (aclAxis > m_deadZone)
					m_currThrust = aclAxis * m_forwardAcl;
				else if (aclAxis < -m_deadZone)
					m_currThrust = aclAxis * m_backwardAcl;

				// Side Thrust
				m_currSideThrust = 0.0f;
				float aclSideAxis = aiController.moveX;
				if (aclSideAxis > m_deadZone)
					m_currSideThrust = aclSideAxis * m_sideAcl;
				else if (aclSideAxis < -m_deadZone)
					m_currSideThrust = aclSideAxis * m_sideAcl;

				// Turning
				m_currTurn = 0.0f;
				float turnAxis = aiController.turn;
				if (Mathf.Abs (turnAxis) > m_deadZone)
					m_currTurn = turnAxis;


				// Firing
				if (aiController.fire && Time.time > nextFire) 
				{
					nextFire = Time.time + fireRate;
					Rumble(0.15f);
					gameObject.rigidbody.AddExplosionForce(explosionPower, shotSpawn.position, explosionRadius);
					tankVelocity = rigidbody.velocity;
					fireParticle.Play();
					createShot (tankVelocity);
					AudioSource.PlayClipAtPoint (sfxFire, shotSpawn.position, 0.5f);
				}
				aiController.AIReset();
			}
		} else { // Player control
			var inputDevice = (InputManager.Devices.Count + 1 > playerNumber) ? InputManager.Devices[playerNumber - 1] : null;
			health.healthscore = healthInt;
			if (hasRespawned && inputDevice != null) 
			{
				if (inputDevice.RightTrigger.IsPressed) 
				{
					foreach (Animator anim in spawnAnimators)
					{
						anim.enabled = true;
						anim.Play(anim.GetCurrentAnimatorStateInfo(0).nameHash,-1,0f);
					}
					
					respawnMessage1.SetActive(false);
					respawnMessage2.SetActive(false);
					spawnActiveTimer = Time.time + 1.0f;
					hasRespawned = false;
				}
			}
			else if (!deathRun && inputDevice != null && spawnActiveTimer < Time.time) 
			{
				// Main Thrust
				m_currThrust = 0.0f;
				float aclAxis = inputDevice.Direction.Y;
				if (aclAxis > m_deadZone)
					m_currThrust = aclAxis * m_forwardAcl;
				else if (aclAxis < -m_deadZone)
					m_currThrust = aclAxis * m_backwardAcl;

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
					gameObject.rigidbody.AddExplosionForce(explosionPower, shotSpawn.position, explosionRadius);
					tankVelocity = rigidbody.velocity;
					fireParticle.Play();
					createShot (tankVelocity);
					AudioSource.PlayClipAtPoint (sfxFire, shotSpawn.position, 0.5f);
				}
			}	
		}

		if (gameObject.transform.position.y <= -100) 
		{
			switch(playerNumber)
			{
				case(1):
					score1.score--;
					break;
				case(2):
					score2.score--;
					break;
				case(3):
					score3.score--;
					break;
				case(4):
					score4.score--;
					break;
			}
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
				AudioSource.PlayClipAtPoint(sfxHit, gameObject.transform.position);
				Vector3 explosionPos = other.gameObject.transform.position;
				Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
				hitParticle.transform.position = other.transform.position;
				hitParticle.Play();
				foreach (Collider hit in colliders) 
				{
					if (hit && hit.rigidbody)
						hit.rigidbody.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
					
				}
				healthInt -= shotControllerCopy.damage;
				Destroy (other.gameObject);				
				Rumble(0.15f);
				if (healthInt <= 0)
				{
					healthInt = 0;
					if (shotControllerCopy.playerNumber == 1)
					{
						score1.score++;

						killedMessage.SetActive(true);

						killedMessage.GetComponentsInChildren<Text>()[0].text = "PLAYER 1 KILLED YOU!";

						// player 1 kill message
						killMessage1.SetActive(true);
						killMessage1.GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + playerNumber + "!";

						if (score1.score >= 5)
						{
							PlayerPrefs.SetInt("Winner", 1);
						}
					}
					if (shotControllerCopy.playerNumber == 2)
					{
						score2.score++;

						killedMessage.SetActive(true);

						killedMessage.GetComponentsInChildren<Text>()[0].text = "PLAYER 2 KILLED YOU!";

						// player 2 kill message
						killMessage2.SetActive(true);
						killMessage2.GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + playerNumber + "!";

						if (score2.score >= 5)
						{
							PlayerPrefs.SetInt("Winner", 2);
						}
					}
					if (shotControllerCopy.playerNumber == 3)
					{
						score3.score++;

						killedMessage.SetActive(true);

						killedMessage.GetComponentsInChildren<Text>()[0].text = "PLAYER 3 KILLED YOU!";

						// player 3 kill message
						killMessage3.SetActive(true);
						killMessage3.GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + playerNumber + "!";

						if (score3.score >= 5)
						{
							PlayerPrefs.SetInt("Winner", 3);
						}
					}
					if (shotControllerCopy.playerNumber == 4)
					{
						score4.score++;

						killedMessage.SetActive(true);

						killedMessage.GetComponentsInChildren<Text>()[0].text = "PLAYER 4 KILLED YOU!";

						// player 4 kill message
						killMessage4.SetActive(true);
						killMessage4.GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + playerNumber + "!";

						if (score4.score >= 5)
						{
							PlayerPrefs.SetInt("Winner", 4);
						}
					}
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
		GameObject zBullet = (GameObject)Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		zBullet.GetComponent<ShotController> ().SetVelocity ();
	}

	void Death()
	{
		tempHoverForce = m_hoverForce;
		m_hoverForce = 0.0f;

		m_currThrust = 0.0f;
		m_currSideThrust = 0.0f;
		m_currTurn = 0.0f;

		AudioSource.PlayClipAtPoint(sfxDeath, gameObject.transform.position);
		if (killCheer) {
		AudioSource.PlayClipAtPoint(killCheer, gameObject.transform.position);
		}
		timer = 0.0f;
		deathRun = true;
		foreach (ParticleSystem particle in hoverParticles) 
		{
			particle.Stop();
		}
	}

	void Respawn()
	{
		gameObject.transform.position = initialPosition;
		gameObject.transform.rotation = initialRotation;
		m_hoverForce = tempHoverForce;
		foreach (ParticleSystem particle in hoverParticles) 
		{
			if (particle)
				particle.Play();
		}
		healthInt = maxHealth;
		deathRun = false;
		if (score1.score >= 5 || score2.score >= 5 || score3.score >= 5 || score4.score >= 5)
		{
			Application.LoadLevel("Winscreen");
		}
		killedMessage.SetActive(false);
		hasRespawned = true;
		
		respawnMessage1.SetActive(true);
		respawnMessage2.SetActive(true);
	}

	void Rumble(float duration) {
		if (isAI)
			return;
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
