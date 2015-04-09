using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot") 
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}