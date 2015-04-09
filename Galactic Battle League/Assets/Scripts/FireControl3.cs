using UnityEngine;
using System.Collections;

[System.Serializable]

public class FireControl3 : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public AudioClip sfx;
	
	private float nextFire;
	
	void Update ()
	{
		if (Input.GetButton("Fire3") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
			AudioSource.PlayClipAtPoint(sfx, shotSpawn.position);
		}
	}
	
}
