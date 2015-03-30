using UnityEngine;
using System.Collections;

public class Destroy1 : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot2" || other.tag == "Shot3" ||other.tag == "Shot4") 
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}