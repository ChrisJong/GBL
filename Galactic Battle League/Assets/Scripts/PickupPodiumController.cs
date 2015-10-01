using UnityEngine;
using System.Collections;

public class PickupPodiumController : MonoBehaviour 
{
	public GameObject currentPickup;
	public float initiatePickupTime;
	public float respawnTimeDuration;
	private float nextAppearTime;

	// Use this for initialization
	void Start () 
	{
		nextAppearTime = Time.time + initiatePickupTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (nextAppearTime < Time.time) 
		{
			Vector3 spawnPosition = transform.position;
			spawnPosition.y += 2.6f;
			GameObject zPickup = (GameObject)Instantiate (currentPickup, spawnPosition, currentPickup.transform.rotation);
			zPickup.GetComponent<Animator>().Play(0, -1, 0f);
			nextAppearTime = Time.time + respawnTimeDuration;
		}
	}
}
