using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

	Rigidbody elevator;
	float delayTime = 2;
	float upSpeed = 3;
	float downSpeed = 3;
	bool moving = false;
	bool goingUp = false;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (moving) 
		{
			if (goingUp)
			{
				transform.Translate (Vector3.up * Time.deltaTime * upSpeed);
			}
			else
			{
				transform.Translate (Vector3.down * Time.deltaTime * downSpeed);
			}
		}
	}

	void OnTriggerEnter()
	{
		Invoke ("RaiseElevator", delayTime);
	}

	void OnTriggerExit()
	{

	}

	void RaiseElevator()
	{
		print ("Raise elevator");
		moving = true;
		goingUp = true;
	}

	void LowerElevator()
	{
		print ("Lower elevator");
		moving = true;
		goingUp = false;
	}

	void StopElevator()
	{
		print ("Stopping elevator");
		moving = false;
	}
}
