using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour 
{

	private bool loading = false;
	private int frame = 0;

	// Use this for initialization
	void Start () 
	{

	}

	IEnumerator LoadLevel()
	{
		AsyncOperation async = Application.LoadLevelAsync(PlayerPrefs.GetString("Level"));
		while (!async.isDone)
			yield return 0;
		yield return async;
	}
	
	// Update is called once per frame
	void Update () 
	{
		frame ++;
		if (!loading && frame >= 10) 
		{
			AsyncController async;// = new AsyncController();
			async = (AsyncController)ScriptableObject.CreateInstance("AsyncController");
			StartCoroutine (async.LoadLevel ());
			loading = true;
		}
	}
}
