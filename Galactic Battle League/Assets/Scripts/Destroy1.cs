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
	public AudioClip sfx;
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
					if (score2.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 2);
						Application.LoadLevel("EndScreen");
					}
				}
				if (other.tag == "Shot3")
				{
					score3.score++;
					if (score3.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 3);
						Application.LoadLevel("EndScreen");
					}
				}
				if (other.tag == "Shot4")
				{
					score4.score++;
					if (score4.score >= 5)
					{
						PlayerPrefs.SetInt("Winner", 3);
						Application.LoadLevel("EndScreen");
					}
				}
				Death ();
			}
		}
	}

	void Update()
	{
		if (gameObject.transform.position.y <= -100) 
		{
			score1.score--;
			Death ();
		}
	}

	void Death()
	{
		AudioSource.PlayClipAtPoint(sfx, gameObject.transform.position);
		gameObject.transform.position = initialPosition;
		health.healthscore = 3;
	}
}