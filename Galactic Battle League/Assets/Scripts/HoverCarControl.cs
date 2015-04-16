using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class HoverCarControl : MonoBehaviour
{
	Rigidbody m_body;
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

  	int m_layerMask;

	//Death/respawn variables
	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;
	public HealthCounter health;
	public AudioClip sfxDeath;
	Vector3 initialPosition;
	Quaternion initialRotation;

	private float tempHoverForce;
	private float timer = 0.0f;
	private bool deathRun = false;

	//Fire control variables
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public AudioClip sfxFire;
	public Vector3 tankVelocity;
	private float nextFire;

  void Start()
	{
		initialPosition = gameObject.transform.position;
		initialRotation = gameObject.transform.rotation;

    m_body = GetComponent<Rigidbody>();

    m_layerMask = 1 << LayerMask.NameToLayer("Characters");
    m_layerMask = ~m_layerMask;
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
      } else
      {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(hoverPoint.transform.position, 
                       hoverPoint.transform.position - Vector3.up * m_hoverHeight);
      }
    }
  }
	
  void Update()
  {
		if (!deathRun) 
		{
			// Main Thrust
			m_currThrust = 0.0f;
			float aclAxis = Input.GetAxis ("P" + playerNumber +"_Vertical");
			if (aclAxis > m_deadZone)
				m_currThrust = aclAxis * m_forwardAcl;
			else if (aclAxis < -m_deadZone)
				m_currThrust = aclAxis * m_backwardAcl;

			// Side Thrust
			m_currSideThrust = 0.0f;
			float aclSideAxis = Input.GetAxis ("P" + playerNumber +"_Horizontal");
			if (aclSideAxis > m_deadZone)
				m_currSideThrust = aclSideAxis * m_sideAcl;
			else if (aclSideAxis < -m_deadZone)
				m_currSideThrust = aclSideAxis * m_sideAcl;

			// Turning
			m_currTurn = 0.0f;
			float turnAxis = Input.GetAxis ("P" + playerNumber +"_HorizontalTurn");
			if (Mathf.Abs (turnAxis) > m_deadZone)
				m_currTurn = turnAxis;

			if (Input.GetButton ("Fire" + playerNumber) && Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				tankVelocity = rigidbody.velocity;
				createShot (tankVelocity);
			
				AudioSource.PlayClipAtPoint (sfxFire, shotSpawn.position);
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

		if (health.healthscore <= 0) 
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


		// Forward
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
  }

	void OnTriggerEnter (Collider other)
	{
		if ((other.tag == "Shot1" || other.tag == "Shot2" || other.tag == "Shot3" ||other.tag == "Shot4") && (other.tag != "Shot" + playerNumber)) 
		{
			Destroy (other.gameObject);
			health.healthscore--;
			if (health.healthscore <= 0)
			{
				if (other.tag == "Shot1" && playerNumber != 1)
				{
					score1.score++;
					if (score1.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 1);
					}
				}
				if (other.tag == "Shot2" && playerNumber != 2)
				{
					score2.score++;
					if (score2.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 2);
					}
				}
				if (other.tag == "Shot3" && playerNumber != 3)
				{
					score3.score++;
					if (score3.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 3);
					}
				}
				if (other.tag == "Shot4" && playerNumber != 4)
				{
					score4.score++;
					if (score4.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 4);
					}
				}
			}
		}
	}

	void createShot(Vector3 tankVelocity)
	{
		GameObject zBullet = (GameObject)Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		zBullet.GetComponent<ShotController> ().SetVelocity (tankVelocity);
	}

	void Death()
	{
		tempHoverForce = m_hoverForce;
		m_hoverForce = 0.0f;
		AudioSource.PlayClipAtPoint(sfxDeath, gameObject.transform.position);
		timer = 0.0f;
		deathRun = true;
	}

	void Respawn()
	{
		gameObject.transform.position = initialPosition;
		gameObject.transform.rotation = initialRotation;
		m_hoverForce = tempHoverForce;
		health.healthscore = 3;
		deathRun = false;
		if (score1.score >= 5 || score2.score >= 5 || score3.score >= 5 || score4.score >= 5)
		{
			Application.LoadLevel("EndScreen");
		}
	}
}
