using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class UIController : MonoBehaviour 
{
	public static Color player1Colour = new Color(0.75f, 0.13f, 0.15f, 1.0f);
	public static Color player2Colour = new Color(0.10f, 0.31f, 0.55f, 1.0f);
	public static Color player3Colour = new Color(0.31f, 0.55f, 0.29f, 1.0f);
	public static Color player4Colour = new Color(0.97f, 0.62f, 0.14f, 1.0f);

	public ScoreCounter[] scoreText;
	public int[] score;
	public int[] deaths;
	public float[] damage;
	public GameTimer timer;
	
	public GameObject[] killedMessage;
	public GameObject[] killMessage;
	public GameObject[] pickupMessage;
	
	public Color pyreReqColour = player1Colour;
	public Color valkTechColour = player2Colour;
	public Color javDefColour = player3Colour;
	public Color shardIndColour = player4Colour;

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

		deaths = new int[4];
		damage = new float[4];

		if (pyreReqColour != null) {
			player1Colour = pyreReqColour;
		}
		
		if (valkTechColour != null) {
			player2Colour = valkTechColour;
		}
		
		if (javDefColour != null) {
			player3Colour = javDefColour;
		}
		
		if (shardIndColour != null) {
			player4Colour = shardIndColour;
		}
		
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
		deaths [victim - 1]++;
		
		killedMessage[victim-1].SetActive(true);
		Text[] messages = killedMessage[victim-1].GetComponentsInChildren<Text>();
		foreach(Text msg in messages)
		{
			if (msg.name == "Heading")
				msg.text = "You were killed by";
			else if (msg.name == "FactionText")
			{
				msg.text = getFactionName(attacker);
				msg.color = getFactionColour(attacker);
			}
		}

		killMessage[attacker-1].SetActive(true);
		messages = killMessage[attacker - 1].GetComponentsInChildren<Text>();
		foreach(Text msg in messages)
		{
			if (msg.name == "Heading")
				msg.text = "You killed";
			else if (msg.name == "FactionText")
			{
				msg.text = getFactionName(victim);
				msg.color = getFactionColour(victim);
			}
		}

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

	public void DamageCaused(int attacker, float damageValue)
	{
		damage [attacker - 1] += damageValue;
	}

	public void PickupTaken(int player, string message)
	{
		pickupMessage[player-1].SetActive(true);
		pickupMessage[player-1].GetComponentsInChildren<Text>()[0].text = message;
	}

	public void GameOver()
	{
		DetermineRankings ();

		trackingFile = new StreamWriter (fileName, true);
		trackingFile.WriteLine("Winner: " + PlayerPrefs.GetInt ("Position1Player"));
		trackingFile.Close ();
		Application.LoadLevel("WinScoreboard");
	}

	public void DetermineRankings()
	{
		Dictionary<int, int> scoreList = new Dictionary<int, int> ();
		Dictionary<int, int> deathList = new Dictionary<int, int> ();
		Dictionary<int, float> damageList = new Dictionary<int, float> ();
		int position = 1;

		for (int i = 0; i < 4; i++)
		{
			if (score[i] > 0 || deaths[i] > 0 || damage[i] > 0)
			{
				scoreList.Add (i + 1, score[i]);
				deathList.Add (i + 1, deaths[i]);
				damageList.Add(i + 1, damage[i]);
			}

			String keyName = "Position" + (i+1) + "Player";

			if (PlayerPrefs.HasKey(keyName))
			{
				PlayerPrefs.DeleteKey(keyName);
			}
		}

		// If nothing happens during the game, set player 1 as winnner
		if (scoreList.Count == 0) 
		{
			PlayerPrefs.SetInt ("Position1Player", 1);
			PlayerPrefs.SetInt ("Position1Score", 0);
			PlayerPrefs.SetInt ("Position1Deaths", 0);
			PlayerPrefs.SetInt ("Position1Damage", 0);
		}

		while (scoreList.Count > 0) 
		{
			int topPlayer = -1;
			int topScore = -1;
			int topDeaths = -1;
			float topDamage = -1;

			foreach(KeyValuePair<int, int> sc in scoreList)
			{
				if (sc.Value > topScore)
				{
					topPlayer = sc.Key;
					topScore = sc.Value;
					deathList.TryGetValue(sc.Key, out topDeaths);
					damageList.TryGetValue(sc.Key, out topDamage);
				}
				else if (sc.Value == topScore)
				{
					int currentDeaths;
					deathList.TryGetValue(sc.Key, out currentDeaths);

					if (currentDeaths < topDeaths)
					{
						topPlayer = sc.Key;
						topScore = sc.Value;
						deathList.TryGetValue(sc.Key, out topDeaths);
						damageList.TryGetValue(sc.Key, out topDamage);
					}
					else if (currentDeaths == topDeaths)
					{
						float currentDamage;
						damageList.TryGetValue(sc.Key, out currentDamage);

						if (currentDamage > topDamage)
						{
							topPlayer = sc.Key;
							topScore = sc.Value;
							deathList.TryGetValue(sc.Key, out topDeaths);
							damageList.TryGetValue(sc.Key, out topDamage);
						}
					}
				}
			}

			PlayerPrefs.SetInt ("Position" + position + "Player", topPlayer);
			PlayerPrefs.SetInt ("Position" + position + "Score", topScore);
			PlayerPrefs.SetInt ("Position" + position + "Deaths", topDeaths);
			PlayerPrefs.SetInt ("Position" + position + "Damage", (int)topDamage);

			scoreList.Remove(topPlayer);
			deathList.Remove(topPlayer);
			damageList.Remove(topPlayer);
			position++;
		}
	}

	public static string GetFactionName(int playerNumber)
	{
		string name = "";
		
		switch (playerNumber) 
		{
		case 1: name = "PYRE REQUISTIONS";
			break;
		case 2: name = "VALKYRIE TECHNOLOGIES";
			break;
		case 3: name = "JAVELIN DEFENSE";
			break;
		case 4: name = "SHARD INDUSTRIES";
			break;
		}
		
		return name;
	}

	public static Color getFactionColour(int playerNumber)
	{
		Color colour = new Color ();

		switch (playerNumber) 
		{
		case 1: 
			return player1Colour;
			break;
		case 2:
			return player2Colour;
			break;
		case 3: 
			return player3Colour;
			break;
		case 4: 
			return player4Colour;
			break;
		}

		return colour;
	}
}
