using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour 
{
	public AudioClip sfxHit;
	public int shotSpeed;
	public float explosionRadius = 4.0F;
	public float explosionPower = 25000.0F;
	// Use this for initialization
	void Start () 
	{

	}

	public void SetVelocity (Vector3 tankVelocity)
	{
		rigidbody.velocity = tankVelocity + transform.forward * shotSpeed;
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Player") 
		{
			AudioSource.PlayClipAtPoint(sfxHit, gameObject.transform.position);
			Vector3 explosionPos = transform.position;
			Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
			foreach (Collider hit in colliders) {
				if (hit && hit.rigidbody)
					hit.rigidbody.AddExplosionForce(explosionPower, explosionPos, explosionRadius);
				
			}
			Destroy (gameObject);
		}
	}

}
