using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour {

	public GameObject player1Heavy;
	public GameObject player1Light;
	public GameObject player2Heavy;
	public GameObject player2Light;
	public GameObject player3Heavy;
	public GameObject player3Light;
	public GameObject player4Heavy;
	public GameObject player4Light;

	public float moveX;
	public float moveY;
	public float turn;
	public bool fire;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void AIUpdate (int playerNumber) {
	
	}

	// Resets the variables back to original values
	public void AIReset () {

	}
}
