using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour 
{

	public void BeginCharacterSelect () 
	{
		Application.LoadLevel ("CharacterSelect");
	}

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

	public void BeginMainMenu () 
	{
		Application.LoadLevel ("MainMenu");
	}
}

