using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Destroy1 : MonoBehaviour
{
	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;
	public HealthCounter1 health;
	Vector3 initialPosition;

	void Start()
	{
		initialPosition = gameObject.transform.position;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot2" || other.tag == "Shot3" ||other.tag == "Shot4") 
		{
			Destroy (other.gameObject);
			health.healthscore--;
			if (health.healthscore <= 0)
			{
				if (other.tag == "Shot2")
				{
					score2.score++;
				}
				if (other.tag == "Shot3")
				{
					score3.score++;
				}
				if (other.tag == "Shot4")
				{
					score4.score++;
				}
				Death ();
			}
		}
	}

	void Update()
	{
		if (gameObject.transform.position.y <= -100)
			Death ();
	}

	void Death()
	{
		gameObject.transform.position = initialPosition;
		score1.score--;
		health.healthscore = 3;
	}
}