using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreboardController : MonoBehaviour {

	Text player;
	Text score;
	Text deaths;
	Text damage;

	// Use this for initialization
	void Start () 
	{
		string name = gameObject.name;

		if (PlayerPrefs.HasKey(name + "Player"))
		{
			foreach (Transform child in transform)
			{
				if (child.name == "Player")
				{
					player = child.GetComponent<Text>();
					player.text = UIController.getFactionName(PlayerPrefs.GetInt(name + "Player"));
					player.color = UIController.getFactionColour(PlayerPrefs.GetInt(name + "Player"));
				}
				else if (child.name == "Kills")
				{
					score = child.GetComponent<Text>();
					score.text = PlayerPrefs.GetInt(name + "Score").ToString();
				}
				else if (child.name == "Deaths")
				{
					deaths = child.GetComponent<Text>();
					deaths.text = PlayerPrefs.GetInt(name + "Deaths").ToString();
				}
				else if (child.name == "Damage")
				{
					damage = child.GetComponent<Text>();
					damage.text = PlayerPrefs.GetInt(name + "Damage").ToString();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}
}
