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
	
	// AIUpdate updates movex, movey, turn and fire based on location of enemy and such
	public void AIUpdate (int playerNumber) {
		// players is an array to hold the active tank for each player
		GameObject[] players = new GameObject[4] {null, null, null, null};

		// Populate the array of players
		// For each player, if they have selected their light tank, set their index of that array to their light tank, otherwise, set it to their heavy tank
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

		// self is the tank the AI calculations are being done for
		GameObject self = null;
		// enemies is an array of all the other tanks
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

		// closest enemy to self is calculated 
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

		// Direction to closest target
		Vector3 direction = closestTarget.transform.position - self.transform.position;
		// We don't care about height difference
		direction.y = 0;
		// We need a vector of up to 1 magnitude, so normalized for now
		direction = direction.normalized;
		// We need the local direction vector, so InverseTransformDirection
		direction = self.transform.InverseTransformDirection(direction);

		//moveX, moveY set to the appropriate values to make the tank move in the direction vector
		moveX = direction.x;
		moveY = direction.z;
		//Set the direction to turn towards to direction.x (the direction the enemy is in)
		turn = direction.x;
		// if the direction it is facing in is close enough to the direction the enemy is at, fire
		if (direction.z > 0.98)
		{
			fire = true;

		} else {

			fire = false;
		}



	}

	// Resets the variables back to original values
	public void AIReset () {
		moveX = 0.0f;
		moveY = 0.0f;
		turn = 0.0f;
		fire = false;
	}
}
