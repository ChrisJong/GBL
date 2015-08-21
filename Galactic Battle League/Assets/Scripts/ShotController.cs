using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour 
{
	public AudioClip sfxHit;
	public int shotSpeed;
	public float explosionRadius = 4.0F;
	public float explosionPower = 25000.0F;
	public int damage;
	public int playerNumber;
	// Use this for initialization
	void Start () 
	{

	}

	public void SetVelocity ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * shotSpeed;
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Player") 
		{
			AudioSource.PlayClipAtPoint(sfxHit, gameObject.transform.position, 0.25f);
			//Vector3 explosionPos = transform.position;
			//Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
			//foreach (Collider hit in colliders) {
				// if (hit && hit.GetComponent<Rigidbody>())
					// hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionRadius);
				
			//}
			Destroy (gameObject);
		}
	}

}
