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
	GameObject[] players = new GameObject[4] {null, null, null, null};
	public void AIUpdate (int playerNumber) {
		if (PlayerPrefs.GetInt("Player1Tank") == 1) {
			players[0] = player1Light;
		} else {
			players[0] = player1Heavy;
		}
		if (PlayerPrefs.GetInt("Player2Tank") == 1) {
			players[1] = player2Light;
		} else {
			players[1] = player2Heavy;
		}
		if (PlayerPrefs.GetInt("Player3Tank") == 1) {
			players[2] = player3Light;
		} else {
			players[2] = player3Heavy;
		}
		if (PlayerPrefs.GetInt("Player4Tank") == 1) {
			players[3] = player4Light;
		} else {
			players[3] = player4Heavy;
		}

		GameObject self = null;
		GameObject[] enemies = new GameObject[3] {null, null, null};
		for (int i = 0; i < 4; i++)
		{
			if (playerNumber == i+1)
			{
				self = players[i];
			} else if (self) {
				enemies[i-1] = players[i];
			} else {
				enemies[i] = players[i];
			}
		}

		GameObject closestTarget = enemies[0];
		float targetDistance = Vector3.Distance(self.transform.position, enemies[0].transform.position);
		for (int i = 1; i < 3; i++)
		{
			float enemyDistance = Vector3.Distance(self.transform.position, enemies[i].transform.position);
			if (enemyDistance < targetDistance)
			{
				targetDistance = enemyDistance;
				closestTarget = enemies[i];
			}
		}
		Vector3 direction = closestTarget.transform.position - self.transform.position;
		
		direction.y = 0;
		direction = direction.normalized;

		direction = self.transform.InverseTransformDirection(direction);

		moveX = direction.x;
		moveY = direction.z;

	}

	// Resets the variables back to original values
	public void AIReset () {
		moveX = 0.0f;
		moveY = 0.0f;
		turn = 0.0f;
		fire = false;
	}
}
