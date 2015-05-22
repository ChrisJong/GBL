using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WinMessage : MonoBehaviour 
{
	Text winnerText;
	public int score {get; set;}
	// Use this for initialization
	void Start () 
	{
		winnerText = GetComponent<Text> ();
		winnerText.text = "PLAYER " + PlayerPrefs.GetInt("Winner") ;
	}

}
