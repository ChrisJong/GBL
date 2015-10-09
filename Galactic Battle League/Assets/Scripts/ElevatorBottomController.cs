using UnityEngine;
using System.Collections;

public class ElevatorBottomController : MonoBehaviour {

	public ElevatorController elevator;

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
			elevator.BottomTriggered();
		}
	}
}
