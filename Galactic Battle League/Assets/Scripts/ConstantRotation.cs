using UnityEngine;
using System.Collections;

public class ConstantRotation : MonoBehaviour 
{

	void Update ()
	{
		transform.Rotate (0,0,200*Time.deltaTime); //rotates 50 degrees per second around z axis
	}

}