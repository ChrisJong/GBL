using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

	float riseDelayTime = 2;
	float descendDelayTime = 3;
	bool elevatorAtBottom = true;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerEnter()
	{
		if (elevatorAtBottom) 
		{
			elevatorAtBottom = false;
			Invoke ("RaiseElevator", riseDelayTime);
		}
	}

	void RaiseElevator()
	{
		Animator anim = GetComponent<Animator> ();
		anim.Play ("Elevator_Rise");

		Invoke ("LowerElevator", descendDelayTime);
	}

	void LowerElevator()
	{
		Animator anim = GetComponent<Animator> ();
		anim.Play ("Elevator_Fall");
		elevatorAtBottom = true;
	}
}
