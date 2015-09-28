using UnityEngine;
using System.Collections;

public class PickupPodiumController : MonoBehaviour 
{
	public GameObject currentPickup;

	// Use this for initialization
	void Start () 
	{
		currentPickup = null;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void spawnPickup(string pickupName)
	{
		Vector3 spawnPosition = transform.position;
		spawnPosition.y += 2.6f;

		//Debug.Log (pickupName);
		GameObject pickupObjectType = GameObject.Find (pickupName);

		currentPickup = (GameObject)Instantiate (pickupObjectType, spawnPosition, pickupObjectType.transform.rotation);
		currentPickup.GetComponent<Animator>().Play(0, -1, 0f);
	}
}
