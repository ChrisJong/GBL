using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreCounter1 : MonoBehaviour 
{
	Text scoreText;
	public int score {get; set;}
	// Use this for initialization
	void Start () 
	{
		score = 0;
		scoreText = GetComponent<Text> ();
		scoreText.text = "Score: " + score;
	}
	
	// Update is called once per frame
	void Update () 
	{
		scoreText.text = "Score: " + score;
	}
}
