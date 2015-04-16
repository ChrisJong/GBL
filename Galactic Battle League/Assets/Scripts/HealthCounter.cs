using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthCounter : MonoBehaviour 
{
	Text health;
	public int healthscore {get; set;}
	// Use this for initialization
	void Start () 
	{
		healthscore = 3;
		health = GetComponent<Text> ();
		health.text = "Health: " + healthscore;
	}
	
	// Update is called once per frame
	void Update () 
	{
		health.text = "Health: " + healthscore;
	}
}
