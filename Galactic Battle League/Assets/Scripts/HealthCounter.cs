using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthCounter : MonoBehaviour 
{
	public float health {get; set;}
	public float maxHealth {get; set;}
	public RectTransform background;
	Slider[] healthBars;
	// Use this for initialization
	void Start () 
	{
		healthBars = gameObject.GetComponentsInChildren<Slider> ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		background.offsetMin = new Vector2(100 - maxHealth, background.offsetMin.y);
		background.offsetMax = new Vector2(maxHealth - 100, background.offsetMax.y);

		foreach (Slider healthBar in healthBars) {
			healthBar.value = health;
		}
	}
}
