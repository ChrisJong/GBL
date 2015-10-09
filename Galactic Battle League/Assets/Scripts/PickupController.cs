using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupController : MonoBehaviour 
{
	public float timeToAppearInit, timeToAppearAvg, timeToAppearRand;
	private float nextAppearTime;
	private int activePlayerCount;
	private bool[] activePlayers;

	public float healthLargeWeighting;
	public float healthSmallWeighting;
	public float damageIncreaseWeighting;
	public float invincibilityWeighting;
	public float signalJammerWeighting;
	public float speedBoostWeighting;
	public float unlimitedEnergyWeighting;

	public float damageIncreaseDuration;
	public float invincibilityDuration;
	public float signalJammedDuration;
	public float speedBoostedDuration;
	public float unlimitedEnergyDuration;
	public float damageIncreaseValue;
	public float speedBoostedValue;

	// Use this for initialization
	void Start () 
	{
		nextAppearTime = timeToAppearInit + Time.time;
		activePlayers = new bool[4];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nextAppearTime < Time.time) 
		{
			Transform podium = SelectRandomEmptyPodium();

			if (podium != null)
			{
				PickupPodiumController controller = podium.GetComponent<PickupPodiumController>();
				controller.SpawnPickup(SelectRandomPickup());
			}

			float modifiedTimeToAppear = timeToAppearAvg;

			if (activePlayerCount == 3)
				modifiedTimeToAppear /= 1.5f;
			else if (activePlayerCount == 4)
				modifiedTimeToAppear /= 2f;

			nextAppearTime = Time.time + modifiedTimeToAppear + Random.Range (-timeToAppearRand, timeToAppearRand);

			Debug.Log(activePlayerCount);
		}
	}

	Transform SelectRandomEmptyPodium()
	{
		List<Transform> emptyPodiumList = new List<Transform> ();

		foreach (Transform child in transform) 
		{
			PickupPodiumController controller = child.GetComponent<PickupPodiumController>();

			if (controller.currentPickup == null)
			{
				emptyPodiumList.Add (child);
			}
		}

		if (emptyPodiumList.Count > 0) 
		{
			int selection = Random.Range (0, emptyPodiumList.Count - 1);
			return emptyPodiumList [selection];
		} 
		else 
		{
			return null;
		}
	}

	string SelectRandomPickup()
	{
		string pickupType;
		float totalWeighting = healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting + speedBoostWeighting + unlimitedEnergyWeighting;

		float randNumber = Random.Range (0, totalWeighting);

		if (randNumber < healthLargeWeighting) 
		{
			pickupType = "PickUps_HP";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting)) 
		{
			pickupType = "PickUps_DoubleDamage";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting)) 
		{
			pickupType = "PickUps_Shield";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting)) 
		{
			pickupType = "PickUps_Scrambler";
		}
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting + speedBoostWeighting)) 
		{
			pickupType = "PickUps_Boost";
		}
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting + speedBoostWeighting + unlimitedEnergyWeighting)) 
		{
			pickupType = "PickUps_Energy";
		} 

		else 
		{
			pickupType = "PickUps_HP"; //default pickup
		}

		return pickupType;
	}

	public void ActivatePlayer(int playerNumber)
	{
		if (activePlayers [playerNumber - 1] == false) 
		{
			activePlayers [playerNumber - 1] = true;
			activePlayerCount++;
		}
	}
}
