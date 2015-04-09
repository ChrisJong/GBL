using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Destroy4 : MonoBehaviour
{
	public ScoreCounter1 score1;
	public ScoreCounter2 score2;
	public ScoreCounter3 score3;
	public ScoreCounter4 score4;
	public HealthCounter4 health;
	Vector3 initialPosition;

	void Start()
	{
		initialPosition = gameObject.transform.position;
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot1" || other.tag == "Shot2" || other.tag == "Shot3") 
		{
			Destroy (other.gameObject);
			health.healthscore--;
			if (health.healthscore <= 0)
			{
				if (other.tag == "Shot1")
				{
					score1.score++;
				}
				if (other.tag == "Shot2")
				{
					score2.score++;
				}
				if (other.tag == "Shot3")
				{
					score3.score++;
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
		score4.score--;
		health.healthscore = 3;
	}
}