using UnityEngine;
using System.Collections;

public class PickupPodiumController : MonoBehaviour 
{
	public GameObject currentPickup;
	public float spawnHeight;

	// Use this for initialization
	void Start () 
	{
		currentPickup = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
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
