using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour 
{

	public void BeginCharacterSelect () 
	{
		Application.LoadLevel ("CharacterSelect");
	}

	public void BeginLevelSelect () 
	{
		Application.LoadLevel ("LevelSelectMenu");
	}

	public void BeginArena () 
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena");
	}

	public void BeginInferno () 
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena02");
	}

	public void BeginArenaThree ()
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena04");
	}

	public void BeginMainMenu () 
	{
		Application.LoadLevel ("MainMenu");
	}

	public void BeginControls ()
	{
		Application.LoadLevel ("Controls");
	}

	public void BeginWinScreen()
	{
		Application.LoadLevel ("WinScreen");
	}
}

