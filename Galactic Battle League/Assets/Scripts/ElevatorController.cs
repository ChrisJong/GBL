using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

	float doorCloseDelayTime = 1;
	float doorOpenDelayTime = 8;
	float riseDelayTime = 3;
	float descendDelayTime = 12;
	bool elevatorAtBottom = true;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.name == "Hover Car Heavy" || collider.name == "Hover Car Light") 
		{
			if (elevatorAtBottom) 
			{
				elevatorAtBottom = false;
				Invoke ("CloseDoors", doorCloseDelayTime);
			}
		}
	}

	void CloseDoors()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "elevatorDoor_Left")
			{
				Animator anim = child.GetComponent<Animator>();
				anim.Play("elevator_Door_Left");
			}
			else if (child.name == "elevatorDoor_Right")
			{
				Animator anim = child.GetComponent<Animator>();
				anim.Play("elevator_Door_Right");
			}
		}

		Invoke ("RaiseElevator", riseDelayTime);
	}

	void RaiseElevator()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "elevatorInterior") 
			{
				Animator anim = child.GetComponent<Animator> ();
				anim.Play ("elevator_Floor_Rise");
			}
		}
		
		Invoke ("LowerElevator", descendDelayTime);
	}
	
	void LowerElevator()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "elevatorInterior") 
			{
				Animator anim = child.GetComponent<Animator> ();
				anim.Play ("elevator_Floor_Fall");
			}
		}

		Invoke ("OpenDoors", doorOpenDelayTime);
	}

	void OpenDoors()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "elevatorDoor_Left")
			{
				Animator anim = child.GetComponent<Animator>();
				anim.Play("elevator_Door_Left_Open");
			}
			else if (child.name == "elevatorDoor_Right")
			{
				Animator anim = child.GetComponent<Animator>();
				anim.Play("elevator_Door_Right_Open");
			}
		}

		elevatorAtBottom = true;
	}
}
