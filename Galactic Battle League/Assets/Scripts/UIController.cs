using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class UIController : MonoBehaviour 
{
	public ScoreCounter[] scoreText;
	public int[] score;
	public GameTimer timer;
	
	public GameObject[] killedMessage;
	public GameObject[] killMessage;

	private string fileName;
	private StreamWriter trackingFile;

	// Use this for initialization
	void Start () 
	{
		Directory.CreateDirectory("tracking");
		fileName = "tracking\\" + DateTime.Now.ToString("ddMMyyyyHHmm") + "kills.txt";
		trackingFile = new StreamWriter(fileName, true);
		trackingFile.Close ();
		timer = GameObject.Find ("TimerText").GetComponent<GameTimer> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (timer.time == 0)
		{
			GameOver();
		}
	}
	
	public void PlayerKill(int attacker, int attackerClass, int victim, int victimClass) 
	{
		score [attacker - 1]++;
		scoreText[attacker - 1].score++;
		
		killedMessage[victim-1].SetActive(true);
		killedMessage[victim-1].GetComponentsInChildren<Text>()[0].text = "PLAYER " + attacker.ToString() + " KILLED YOU!";
		
		killMessage[attacker-1].SetActive(true);
		killMessage[attacker-1].GetComponentsInChildren<Text>()[0].text = "YOU KILLED PLAYER " + victim.ToString() + "!";

		if (File.Exists (fileName)) 
		{
			trackingFile = new StreamWriter(fileName, true);
			string victimClassString = "NOT FOUND";
			if(victimClass == 1)
				victimClassString = "LIGHT";
			else
				victimClassString = "HEAVY";

			//attacker class just uses damaage of shot. Will need to be reworked for laser.
			string attackerClassString = "NOT FOUND";
			if (attackerClass >= 10)
				attackerClassString = "HEAVY";
			else if (attackerClass >= 2)
				attackerClassString = "LIGHT";
			else
				attackerClassString = "HEAVY";

			trackingFile.WriteLine("Player " + attacker.ToString() + " (" + attackerClassString + ") killed player " + victim.ToString() + " (" + victimClassString + ").");
			trackingFile.WriteLine ("Time Remaining: " + timer.time.ToString());
			trackingFile.Close();
		}

	}


	// Ranks players and stores the 1st/2nd/3rd in the PlayerPrefs
	public void GameOver()
	{

		int[] playerNumbers = {1,2,3,4};
		//int highScore = -1;
		//int highPlayer = 0;

		//for (int i = 0; i < 4; i++) {
		//	if (score[i] > highScore)
		//	{
		//		highScore = score[i];
		//		highPlayer = i + 1;
		//	}
		//}


		Array.Sort (score, playerNumbers);

		PlayerPrefs.SetInt ("Winner", playerNumbers[3]);
		PlayerPrefs.SetInt ("Second", playerNumbers[2]);
		PlayerPrefs.SetInt ("Third", playerNumbers[1]);
		trackingFile = new StreamWriter (fileName, true);
		trackingFile.WriteLine("Winner: " + playerNumbers[3].ToString());
		trackingFile.Close ();
		Application.LoadLevel("WinScreen");
	}
}
