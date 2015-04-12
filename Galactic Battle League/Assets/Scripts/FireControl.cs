using UnityEngine;
using System.Collections;

[System.Serializable]

public class FireControl : MonoBehaviour
{
	public GameObject shot;
	public Transform shotSpawn;
	public float fireRate;
	public AudioClip sfx;
	public int playerNumber; 
	public Vector3 tankVelocity;
	private float nextFire;

	void Update ()
	{
		if (Input.GetButton("Fire" + playerNumber) && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			tankVelocity = rigidbody.velocity;
			createShot(tankVelocity);

			AudioSource.PlayClipAtPoint(sfx, shotSpawn.position);
		}
	}

	void createShot(Vector3 tankVelocity)
	{
		GameObject zBullet = (GameObject)Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
		zBullet.GetComponent<ShotController> ().SetVelocity (tankVelocity);
	}
}
