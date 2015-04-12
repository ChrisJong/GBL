using UnityEngine;
using System.Collections;

public class ShotController : MonoBehaviour 
{
	public int shotSpeed;
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
		if (other.tag == "Wall")
			Destroy (gameObject);
	}

}
