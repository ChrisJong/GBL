using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour 
{
	Text winnerText;
	Text secondText;
	Text thirdText;
	Image winnerLogo1;
	Image winnerLogo2;

	public int score {get; set;}
	// Use this for initialization
	void Start () 
	{
		int winner = PlayerPrefs.GetInt ("Position1Player");

		switch (winner) 
		{
		case 1: winnerLogo1 = GameObject.Find ("Pirate_Logo1").GetComponent<Image>();
				winnerLogo2 = GameObject.Find ("Pirate_Logo2").GetComponent<Image>();
				break;
		case 2: winnerLogo1 = GameObject.Find ("Tech_Logo1").GetComponent<Image>();
				winnerLogo2 = GameObject.Find ("Tech_Logo2").GetComponent<Image>();
				break;
		case 3: winnerLogo1 = GameObject.Find ("Military_Logo1").GetComponent<Image>();
				winnerLogo2 = GameObject.Find ("Military_Logo2").GetComponent<Image>();
				break;
		case 4: winnerLogo1 = GameObject.Find ("Industry_Logo1").GetComponent<Image>();
				winnerLogo2 = GameObject.Find ("Industry_Logo2").GetComponent<Image>();
				break;
		}

		winnerLogo1.enabled = true;
		winnerLogo2.enabled = true;

		winnerText = GetComponent<Text> ();
		winnerText.text = UIController.getFactionName (winner);
		
		secondText = GameObject.Find ("SecondText").GetComponent<Text> ();
		secondText.text = UIController.getFactionName(PlayerPrefs.GetInt("Position2Player"));

		thirdText = GameObject.Find ("ThirdText").GetComponent<Text> ();
		thirdText.text = UIController.getFactionName(PlayerPrefs.GetInt("Position3Player"));

		if (PlayerPrefs.HasKey("Position3Player") == false)
		{
			MeshRenderer trophy = GameObject.Find ("3rdTrophy").GetComponent<MeshRenderer> ();
			trophy.enabled = false;
		}
	}
}
