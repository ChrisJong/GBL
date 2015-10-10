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

	public void BeginPyre () 
	{
		Application.LoadLevel ("Bckdrp_pyre");
	}

	public void BeginValk () 
	{
		Application.LoadLevel ("Bckdrp_Valk");
	}

	public void BeginJav () 
	{
		Application.LoadLevel ("Bckdrp_Jav");
	}

	public void BeginShard () 
	{
		Application.LoadLevel ("Bckdrp_Shard");
	}

	public void BeginBlank () 
	{
		Application.LoadLevel ("Bckdrp_Blank");
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

	public void BeginWinScreen()
	{
		Application.LoadLevel ("WinScreen");
	}
}

