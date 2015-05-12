using UnityEngine;
using System.Collections;

public class SelectType : MonoBehaviour 
{



	public void lightOne () 
	{
		PlayerPrefs.SetInt ("Player1Tank", 1);
	}

	public void heavyOne () 
	{
		PlayerPrefs.SetInt ("Player1Tank", 2);
	}
	

	public void lightTwo () 
	{
		PlayerPrefs.SetInt ("Player2Tank", 1);
	}

	public void heavyTwo () 
	{
		PlayerPrefs.SetInt ("Player2Tank", 2);
	}


		public void lightThree () 
	{
		PlayerPrefs.SetInt ("Player3Tank", 1);
	}

	public void heavyThree () 
	{
		PlayerPrefs.SetInt ("Player3Tank", 2);
	}


	public void lightFour () 
	{
		PlayerPrefs.SetInt ("Player4Tank", 1);
	}

	public void heavyFour () 
	{
		PlayerPrefs.SetInt ("Player4Tank", 2);
	}
}

