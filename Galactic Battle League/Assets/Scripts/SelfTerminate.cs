using UnityEngine;
using System.Collections;

public class SelfTerminate : MonoBehaviour 
{
	public float death;
	private float deathTime;

	// Use this for initialization
	void Start () 
	{
		deathTime = Time.time + death;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Time.time > deathTime) 
		{
			Destroy (gameObject);
		}
	}
}
