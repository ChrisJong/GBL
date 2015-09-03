using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
	public ScoreCounter[] scoreText;
	public int[] score;
	public GameTimer timer;
	
	public GameObject[] killedMessage;
	public GameObject[] killMessage;
	
	// Use this for initialization
	void Start () 
	{
		timer = GameObject.Find ("TimerText").GetComponent<GameTimer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (score[0] >= 5 || score[1] >= 5 || score[2] >= 5 || score[3] >= 5 || timer.time == 0)
		{
			GameOver();
		}
	}
	
	public void PlayerKill(int attacker, int victim) 
	{
		score [attacker - 1]++;
		scoreText[attacker - 1].score++;
		
		killedMessage[victim-1].SetActive(true);
		killedMessage[victim-1].GetComponentsInChildren<Text>()[0].text = "PLAYER " + attacker.ToString() + " KILLED YOU!";
		
		killMessage[attacker-1].SetActive(true);
		killMessage[attacker-1].GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + victim.ToString() + "!";
		
		if (score[attacker-1] >= 5)
		{
			GameOver();
		}
	}


	// Ranks players and stores the 1st/2nd/3rd in the PlayerPrefs
	public void GameOver()
	{
		int[] playerNumbers = {1,2,3,4};
		Array.Sort (score, playerNumbers);

		PlayerPrefs.SetInt ("Winner", playerNumbers[3]);
		PlayerPrefs.SetInt ("Second", playerNumbers[2]);
		PlayerPrefs.SetInt ("Third", playerNumbers[1]);
		Application.LoadLevel("WinScreen");
	}
}
