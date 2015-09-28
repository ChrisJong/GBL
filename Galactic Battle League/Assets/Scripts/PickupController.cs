using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PickupController : MonoBehaviour 
{
	//public Vector3 spawnArea1, spawnArea2;
	public float timeToAppearInit, timeToAppearAvg, timeToAppearRand;
	private float nextAppearTime;
	//public GameObject pickup;

	public float healthLargeWeighting;
	public float healthSmallWeighting;
	public float damageIncreaseWeighting;
	public float invincibilityWeighting;
	public float signalJammerWeighting;

	// Use this for initialization
	void Start () 
	{
		nextAppearTime = timeToAppearInit + Time.time;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nextAppearTime < Time.time) 
		{
			Transform podium = selectRandomEmptyPodium();

			if (podium != null)
			{
				PickupPodiumController controller = podium.GetComponent<PickupPodiumController>();
				controller.spawnPickup(selectRandomPickup());
				nextAppearTime = Time.time + timeToAppearAvg + Random.Range (-timeToAppearRand, timeToAppearRand);
			}
		}
	}

	Transform selectRandomEmptyPodium()
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

	string selectRandomPickup()
	{
		string pickupType;
		float totalWeighting = healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting;

		float randNumber = Random.Range (0, totalWeighting);

		if (randNumber < healthLargeWeighting) 
		{
			pickupType = "PickupHealthLarge";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting)) 
		{
			pickupType = "PickupHealthSmall";
		}
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting)) 
		{
			pickupType = "PickupDamageIncrease";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting)) 
		{
			pickupType = "PickupInvincibility";
		} 
		else if (randNumber < (healthLargeWeighting + healthSmallWeighting + damageIncreaseWeighting + invincibilityWeighting + signalJammerWeighting)) 
		{
			pickupType = "PickupSignalJammer";
		} 
		else 
		{
			pickupType = "PickupHealthSmall"; //default pickup
		}

		return pickupType;
	}
}
