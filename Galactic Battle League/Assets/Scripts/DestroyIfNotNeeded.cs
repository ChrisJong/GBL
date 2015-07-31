using UnityEngine;
using System.Collections;
using InControl;

public class DestroyIfNotNeeded : MonoBehaviour {
	void Start () {
		if(GameObject.Find(gameObject.name + "NEEDED")) {
			GameObject.Destroy(gameObject);
		} else {
			gameObject.name = gameObject.name + "NEEDED";
			Object.Destroy(gameObject.GetComponent<DestroyIfNotNeeded>());
		}
	}
}