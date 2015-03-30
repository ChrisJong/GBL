using UnityEngine;
using System.Collections;

public class Destroy4 : MonoBehaviour
{
	
	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Shot2" || other.tag == "Shot3" ||other.tag == "Shot1") 
		{
			Destroy (other.gameObject);
			Destroy (gameObject);
		}
	}
}