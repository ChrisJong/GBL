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

	public void BeginMainStage () 
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena");
	}

	public void BeginAltitude () 
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("Altitude");
	}

	public void BeginCityscape ()
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("arena04");
	}

	public void BeginCrucible ()
	{
		GameObject.Destroy(GameObject.Find("MenuMusic"));
		Application.LoadLevel ("Crucible");
	}

	public void BeginMainMenu () 
	{
		Application.LoadLevel ("MainMenu");
	}

	public void BeginControls ()
	{
		Application.LoadLevel ("Controls");
	}
}

