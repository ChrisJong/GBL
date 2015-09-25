using UnityEngine;
using System.Collections;

public class PickupController : MonoBehaviour 
{
	public Vector3 spawnArea1, spawnArea2;
	public float timeToAppearInit, timeToAppearAvg, timeToAppearRand;
	private float nextAppearTime;
	public GameObject pickup;
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
			float spawnX = Random.Range(spawnArea1.x, spawnArea2.x);
			float spawnY = Random.Range(spawnArea1.y, spawnArea2.y);
			float spawnZ = Random.Range(spawnArea1.z, spawnArea2.z);
			Vector3 pickupSpawn = new Vector3(spawnX, spawnY, spawnZ);
			GameObject zPickup = (GameObject)Instantiate (pickup, pickupSpawn, pickup.transform.rotation);
			zPickup.GetComponent<Animator>().Play(0, -1, 0f);;
			nextAppearTime = Time.time + timeToAppearAvg + Random.Range (-timeToAppearRand, timeToAppearRand);
		}
	}
}
