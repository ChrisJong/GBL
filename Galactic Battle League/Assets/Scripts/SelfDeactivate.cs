using UnityEngine;
using System.Collections;

public class SelfDeactivate : MonoBehaviour 
{
	public float deactivate;
	private float deactivateTime;

	// Use this for initialization
	void OnEnable () 
	{
		deactivateTime = Time.time + deactivate;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > deactivateTime) 
		{
			gameObject.SetActive(false);
		}
	}
}
