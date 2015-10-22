using UnityEngine;
using System.Collections;

public class PickupPodiumController : MonoBehaviour 
{
	public GameObject pickupType;
	public GameObject currentPickup;
	public float spawnHeight;
	public float initialSpawnTime;
	public float respawnTime;
	private float nextRespawnTime;

	// Use this for initialization
	void Start () 
	{
		nextRespawnTime = Time.time + initialSpawnTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nextRespawnTime < Time.time) 
		{
			Vector3 spawnPosition = transform.position;
			spawnPosition.y += spawnHeight;

			currentPickup = (GameObject)Instantiate (pickupType, spawnPosition, pickupType.transform.rotation);
			currentPickup.GetComponent<Animator>().Play(0, -1, 0f);

			nextRespawnTime = Time.time + respawnTime;
		}
	}

	public void SpawnPickup(string pickupName)
	{
		Vector3 spawnPosition = transform.position;
		spawnPosition.y += spawnHeight;

		GameObject pickupObjectType = GameObject.Find (pickupName);

		currentPickup = (GameObject)Instantiate (pickupObjectType, spawnPosition, pickupObjectType.transform.rotation);
		currentPickup.GetComponent<Animator>().Play(0, -1, 0f);
	}
}
