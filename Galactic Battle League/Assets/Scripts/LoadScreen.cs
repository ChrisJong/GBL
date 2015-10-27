using UnityEngine;
using System.Collections;

public class LoadScreen : MonoBehaviour 
{

	private bool loading = false;
	private float loadDelay;

	// Use this for initialization
	void Start () 
	{
		loadDelay = Time.time + 3;
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
		if (!loading && loadDelay < Time.time) 
		{
			AsyncController async;// = new AsyncController();
			async = (AsyncController)ScriptableObject.CreateInstance("AsyncController");
			StartCoroutine (async.LoadLevel ());
			loading = true;
		}
	}
}
