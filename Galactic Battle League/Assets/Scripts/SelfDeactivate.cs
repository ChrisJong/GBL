using UnityEngine;
using System.Collections;

public class SelfDeactivate : MonoBehaviour 
{
	public float deactivate;

	// Use this for initialization
	void OnEnable () 
	{
		Invoke("Deactivate", deactivate);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void Deactivate(){
		gameObject.SetActive(false);
	}
}
