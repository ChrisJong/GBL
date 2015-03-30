using UnityEngine;
using System.Collections;

public class Destroy3 : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot2" || other.tag == "Shot1" ||other.tag == "Shot4") 
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}