using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour 
{
	public Animator piston;
	public Animator door1;
	public Animator door2;

	private int pistonAnimation;
	private int door1Animation;
	private int door2Animation;

	private double time;
	private bool done;

	void Start()
	{
		AnimatorStateInfo pistonInfo = piston.GetCurrentAnimatorStateInfo(0);
		pistonAnimation = pistonInfo.nameHash;
		AnimatorStateInfo door1Info = door1.GetCurrentAnimatorStateInfo(0);
		door1Animation = door1Info.nameHash;
		AnimatorStateInfo door2Info = door2.GetCurrentAnimatorStateInfo(0);
		door2Animation = door2Info.nameHash;
		print (pistonInfo);
		print (door1Animation.ToString());
		print (door2Animation.ToString());
		piston.enabled = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (Time.time > time)
			done = true;
	}

	void OnCollisionEnter(Collision collision)
	{
		if (false) 
		{
			time = Time.time + 2;
			piston.Play (pistonAnimation, -1, 0f);
			door1.Play(door1Animation, -1, 0f);
			door2.Play(door2Animation, -1, 0f);
			done = false;
		}
	}
}
