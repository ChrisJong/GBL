using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCounter3 : MonoBehaviour 
{
	Text scoreText;
	public int score {get; set;}
	// Use this for initialization
	void Start () 
	{
		score = 0;
		scoreText = GetComponent<Text> ();
		scoreText.text = " " + score;
	}
	
	// Update is called once per frame
	void Update () 
	{
		scoreText.text = " " + score;
	}
}
