using UnityEngine;
using System.Collections;
using System;

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
		
		//Sets aimdirection (the direction the turret needs to aim at) using the aim function	
		Vector3 aimDirection = aim(self, closestTarget);
		aimDirection = self.transform.InverseTransformDirection(aimDirection);

		if (aimDirection.x > 0){
			turn = 1;
		}else if (aimDirection.x < 0){
			turn = -1;
		}
		
		if (aimDirection.z > 0.999){
			fire = true;
		}


	}

	// Resets the variables back to original values
	public void AIReset () {
		moveX = 0.0f;
		moveY = 0.0f;
		turn = 0.0f;
		fire = false;


	}

	//Returns aimDirection, the direction the hovertank needs to aim in
	public Vector3 aim (GameObject self, GameObject target) {
		Vector3 aimDirection = new Vector3();

		Vector3 aimPos;

		float aimDistance;

		float bulletDistance;

		float time = 1.0f;

		bool aimed = false;

		while (!aimed)
		{
			aimPos = target.transform.position + target.GetComponent<Rigidbody>().velocity * time;

			aimDistance = Vector3.Distance(self.transform.position, target.transform.position);

			bulletDistance = 100 * time;

			aimDirection = aimPos - self.transform.position;

			if (Math.Abs(aimDistance - bulletDistance) < 0.2)
			{
				aimed = true;
				return aimDirection;
			}else if (aimDistance > bulletDistance){
				time = time * 1.2f;
			} else if (aimDistance < bulletDistance){
				time = time * 0.8f;
			}
		}
		return aimDirection;

	}
}
