using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyCounter : MonoBehaviour {
	public float energy {get; set;}
	public float maxEnergy {get; set;}
	Slider energyBar;

	// Use this for initialization
	void Start () {
		energyBar = gameObject.GetComponentInChildren<Slider> ();
	}
	
	// Update is called once per frame
	void Update () {
		energyBar.value = energy;
		energyBar.maxValue = maxEnergy;
	}
}
