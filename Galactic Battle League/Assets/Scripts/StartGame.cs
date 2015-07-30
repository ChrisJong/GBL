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
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena");
	}

	public void BeginInferno () 
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("inferno");
	}

	public void BeginMainMenu () 
	{
		Application.LoadLevel ("MainMenu");
	}

	public void BeginControls ()
	{
		Application.LoadLevel ("ControlsController");
	}
}

