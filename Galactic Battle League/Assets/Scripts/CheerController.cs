using UnityEngine;
using System.Collections;

public class CheerController : MonoBehaviour {
	public float lowVolume = 0.1f;
	public float highVolume = 0.3f;
	private float cheerTime;
	public AudioSource cheerSFX;

	// Update is called once per frame
	void Update () {
		if (Time.time > cheerTime)
		{
			cheerSFX.volume = lowVolume;
		}
	}

	void Cheer () {
		cheerTime = Time.time + 1.0f;
		cheerSFX.volume = highVolume;
	}
}
