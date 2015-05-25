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
		health = GetComponent<Text> ();
		health.text = " " + healthscore;
	}
	
	// Update is called once per frame
	void Update () 
	{
		health.text = " " + healthscore;
	}
}
