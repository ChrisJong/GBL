using UnityEngine;
using System.Collections;

public class RestartGame : MonoBehaviour 
{

	public void BeginGame () 
	{
		Application.LoadLevel ("OpeningScreen");
	}
}

