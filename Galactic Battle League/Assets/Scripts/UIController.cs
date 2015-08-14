using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
	public ScoreCounter[] scoreText;
	public int[] score;
	
	public GameObject[] killedMessage;
	public GameObject[] killMessage;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (score[0] >= 5 || score[1] >= 5 || score[2] >= 5 || score[3] >= 5)
		{
			Application.LoadLevel("WinscreenController");
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
			PlayerPrefs.SetInt("Winner", attacker);
			Application.LoadLevel("WinscreenController");
		}
		
	}
}
