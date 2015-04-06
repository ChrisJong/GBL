using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Destroy2 : MonoBehaviour
{
	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;
	public HealthCounter2 health;
	Vector3 initialPosition;

	void OnAwake()
	{
		initialPosition = gameObject.transform.position;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot1" || other.tag == "Shot3" ||other.tag == "Shot4") 
		{
			Destroy (other.gameObject);
			health.healthscore--;
			if (health.healthscore <= 0)
			{
				if (other.tag == "Shot1")
				{
					score1.score++;
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
		score2.score--;
	}
}
