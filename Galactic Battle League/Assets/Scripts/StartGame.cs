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
		PlayerPrefs.SetString ("Level", "arena");
		Application.LoadLevel ("LoadScreen");
	}

	public void BeginAltitude () 
	{
		PlayerPrefs.SetString ("Level", "Altitude");
		Application.LoadLevel ("LoadScreen");
	}

	public void BeginCityscape ()
	{
		PlayerPrefs.SetString ("Level", "arena04");
		Application.LoadLevel ("LoadScreen");
	}

	public void BeginCrucible ()
	{
		PlayerPrefs.SetString ("Level", "Crucible");
		Application.LoadLevel ("LoadScreen");
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

	public void BeginWinScoreboard()
	{
		Application.LoadLevel ("WinScoreboard");
	}
}

