using UnityEngine;
using System.Collections;

public class SelfTerminate : MonoBehaviour 
{
	public float death;

	// Use this for initialization
	void Start () 
	{
		Destroy (gameObject, death);
	}
	
	// Update is called once per frame
	void Update () 
	{
	}
}
