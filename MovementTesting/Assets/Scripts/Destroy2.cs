using UnityEngine;
using System.Collections;

public class Destroy2 : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot1")// || other.tag == "Shot3" ||other.tag == "Shot4") 
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}