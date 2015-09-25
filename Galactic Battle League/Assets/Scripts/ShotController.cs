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
	private ParticleSystem smoke;
	public Vector3 startPoint;
	// Use this for initialization
	void Start () 
	{
		smoke = GetComponentInChildren<ParticleSystem> ();
		startPoint = this.transform.position;
	}

	public void SetVelocity ()
	{
		GetComponent<Rigidbody>().velocity = transform.forward * shotSpeed;
	}
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag != "Player" && other.tag != "Pickup") 
		{
			AudioSource.PlayClipAtPoint(sfxHit, gameObject.transform.position, 0.25f);
			//Vector3 explosionPos = transform.position;
			//Collider[] colliders = Physics.OverlapSphere(explosionPos, explosionRadius);
			//foreach (Collider hit in colliders) {
				// if (hit && hit.GetComponent<Rigidbody>())
					// hit.GetComponent<Rigidbody>().AddExplosionForce(explosionPower, explosionPos, explosionRadius);
				
			//}

			//Separate smoke trail from shot, so smoke disperses normally
			smoke.enableEmission = false;
			smoke.transform.parent=null;
			Destroy(smoke, 3);
			Destroy(gameObject);
		}
	}
}
