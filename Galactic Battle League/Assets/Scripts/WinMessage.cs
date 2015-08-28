using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour 
{
	Text winnerText;
	Text secondText;
	Text thirdText;

	public int score {get; set;}
	// Use this for initialization
	void Start () 
	{
		winnerText = GetComponent<Text> ();
		winnerText.text = "PLAYER " + PlayerPrefs.GetInt("Winner") ;

		secondText = GameObject.Find ("SecondText").GetComponent<Text> ();
		secondText.text = "PLAYER " + PlayerPrefs.GetInt("Second") ;

		thirdText = GameObject.Find ("ThirdText").GetComponent<Text> ();
		thirdText.text = "PLAYER " + PlayerPrefs.GetInt("Third") ;
	}

}
