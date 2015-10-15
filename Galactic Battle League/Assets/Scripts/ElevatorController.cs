using UnityEngine;
using System.Collections;

public class ElevatorController : MonoBehaviour {

	float doorCloseDelayTime = 0.5f;
	float doorOpenDelayTime = 1.5f;
	float riseDelayTime = 0.75f;
	float descendDelayTime = 0.5f;
	bool elevatorAtBottom = true;
	bool elevatorAtTop = false;
	bool doorsClosing = false;
	public Light lt;
	public Color grn;
	public Color rd;
	public GameObject[] players;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (elevatorAtTop) 
		{
			if (CheckIfEmpty())
			{
				Invoke ("LowerElevator", descendDelayTime);
				elevatorAtTop = false;
			}
		}
		
		if (doorsClosing) 
		{
			CloseDoors();
		}
	}

	public void BottomTriggered()
	{
		if (elevatorAtBottom) 
		{
			elevatorAtBottom = false;
			Invoke ("BeginCloseDoors", doorCloseDelayTime);
		}
	}

	void BeginCloseDoors()
	{
		doorsClosing = true;
	}

	void CloseDoors()
	{
		if (IsSafeToClose()) 
		{
			doorsClosing = false;

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
			
			lt.color = rd;

			Invoke ("RaiseElevator", riseDelayTime);
		}
	}

	void RaiseElevator()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "elevatorInterior") 
			{
				Animator anim = child.GetComponent<Animator> ();
				anim.Play ("elevator_Floor_Rise");
				Invoke ("ElevatorRisen", 3);
			}
		}
	}

	void ElevatorRisen()
	{
		elevatorAtTop = true;
	}

	bool CheckIfEmpty()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "TopCollider") 
			{
				Collider collider = child.GetComponent<Collider>();

				foreach (GameObject player in players)
				{
					foreach (Transform playerComponent in player.transform)
					{
						if (playerComponent.name == "Hover Car Heavy" || playerComponent.name == "Hover Car Light")
						{
							if (collider.bounds.Contains(playerComponent.transform.position))
							{
								return false;
							}
						}
					}
				}
			}
		}

		return true;
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
	
		lt.color = grn;
		Invoke ("ElevatorDescended", 3);
	}

	void ElevatorDescended()
	{
		elevatorAtBottom = true;
	}

	bool IsSafeToClose()
	{
		foreach (Transform child in transform) 
		{
			if (child.name == "DoorCloseCollider") 
			{
				Collider collider = child.GetComponent<Collider>();
				
				foreach (GameObject player in players)
				{
					foreach (Transform playerComponent in player.transform)
					{
						if (playerComponent.name == "Hover Car Heavy" || playerComponent.name == "Hover Car Light")
						{
							if (collider.bounds.Contains(playerComponent.transform.position))
							{
								return false;
							}
						}
					}
				}
			}
		}

		return true;
	}
}
