using UnityEngine;
using System.Collections;

public class AsyncController : ScriptableObject {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public IEnumerator LoadLevel()
	{
		AsyncOperation async = Application.LoadLevelAsync(PlayerPrefs.GetString("Level"));
		//while (!async.isDone)
		//	yield return 0;
		yield return async;
	}
}
