using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour 
{

	public void BeginGame () 
	{
		Application.LoadLevel ("LevelSelectMenu");
	}

	public void BeginArena () 
	{
		Application.LoadLevel ("arena");
	}

	public void BeginInferno () 
	{
		Application.LoadLevel ("inferno");
	}
}

