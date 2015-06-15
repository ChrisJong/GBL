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

	public float wanderJitter = 0.2f;
	public float wanderRadius = 2.0f;
	public float wanderDistance = 2.2f;

//	 Vector3 player1WanderTarget = Vector3.forward;
//	 Vector3 player2WanderTarget = Vector3.forward;
//	 Vector3 player3WanderTarget = Vector3.forward;
//	 Vector3 player4WanderTarget = Vector3.forward;

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
		GameObject closestTarget = null;
		float targetDistance = Mathf.Infinity;
		RaycastHit hit;
		Vector3 rayOrigin = self.transform.position;
		rayOrigin.y += 1.4f;
		for (int i = 0; i < 3; i++)
		{
			if (Physics.Raycast(rayOrigin, enemies[i].transform.position - self.transform.position, out hit))
			{
				if (hit.distance < targetDistance)
				{
					if(hit.collider.tag == "Player" && !hit.collider.GetComponent<HoverCarControl>().deathRun){
						targetDistance = hit.distance;
						closestTarget = hit.collider.gameObject;
					}
				}				
			}
		}



		if (closestTarget != null) {
			Vector3 flankingTarget = closestTarget.transform.position + closestTarget.transform.right * -10;
			Vector3 direction = seek(self, flankingTarget);
			// We need the local direction vector, so InverseTransformDirection
			direction = self.transform.InverseTransformDirection(direction);

			// Wall avoidance
			direction += wallAvoidance(self)*2;
			direction = direction.normalized;

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
			
			if (aimDirection.normalized.z > 0.999) {
				fire = true;
			} else if (Physics.Raycast(rayOrigin, self.transform.forward, out hit, 10.0f)) {
				if (hit.collider.tag == "Player" && !hit.collider.GetComponent<HoverCarControl>().deathRun)
				{
					fire = true;
				}
			}
		} else {
			// get wander vector
			// Vector3 direction = wander(self, playerNumber);

			// Super Wall Avoidance
			Vector3 direction = OpenSpaceSeek(self);
			direction = self.transform.InverseTransformDirection(direction);

			// Wall avoidance
			direction += wallAvoidance(self)*2;
			direction = direction.normalized;

			// moveX, moveY set to the appropriate values to make the tank move in the direction vector
			moveX = direction.x;
			moveY = direction.z;

			// Always be turning
			turn = 0.5f;
		}
	}

	// Resets the variables back to original values
	public void AIReset () {
		moveX = 0.0f;
		moveY = 0.0f;
		turn = 0.0f;
		fire = false;
	}

	// Returns a steering direction (world space, normalised) to lead the agent to the target
	Vector3 seek(GameObject self, Vector3 target) {
		// Direction to closest target
		Vector3 direction = target - self.transform.position;
		// We don't care about height difference
		direction.y = 0;
		// We just want the direction, so normalized for now
		direction = direction.normalized;

		// Our current direction
		Vector3 currentDir = self.GetComponent<Rigidbody>().velocity.normalized;

		return (direction * 2 - currentDir).normalized;
	}

	//Returns aimDirection, the direction the hovertank needs to aim in
	Vector3 aim (GameObject self, GameObject target) {
		Vector3 aimDirection = new Vector3();

		Vector3 aimPos;

		float aimDistance;

		float bulletDistance;

		float time = 1.0f;

		bool aimed = false;

		while (!aimed)
		{
			// calculating all of the starting variables
			aimPos = target.transform.position + target.GetComponent<Rigidbody>().velocity * time;

			aimDistance = Vector3.Distance(self.transform.position, target.transform.position);
			//bullet distance is bullet speed multiplied by time, a constant number is currently used as at the moment bullet speed is a constant. this might be changed in the future to allow heavy and light tanks to have different bullet speeds
			bulletDistance = 100 * time;

			aimDirection = aimPos - self.transform.position;
			//testing if the time has been estimated correctly
			if (Mathf.Abs(aimDistance - bulletDistance) < 3.0f)
			{
				aimed = true;
				return aimDirection;
			//increments or decrements time based on whether it was estimating under or over
			}else if (aimDistance > bulletDistance){
				time = time * 1.2f;
			} else if (aimDistance < bulletDistance){
				time = time * 0.8f;
			}
		}
		return aimDirection;

	}

//	// Returns a direction for wandering, local space, normalised
//	Vector3 wander(GameObject self, int playerNumber) {
//		// Get the correct player's wander target
//		Vector3 wanderTarget;
//		if (playerNumber == 1) {
//			wanderTarget = player1WanderTarget;
//		} else if (playerNumber == 2) {
//			wanderTarget = player2WanderTarget;
//		} else if (playerNumber == 3) {
//			wanderTarget = player3WanderTarget;
//		} else {
//			wanderTarget = player4WanderTarget;
//		}
//
//		wanderTarget.x += UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter;
//		wanderTarget.z += UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter;
//
//		// project onto unit circle
//		wanderTarget = wanderTarget.normalized;
//
//		// increase circle size
//		wanderTarget *= wanderRadius;
//
//		// Store the wander target for this player
//		if (playerNumber == 1) {
//			player1WanderTarget = wanderTarget;
//		} else if (playerNumber == 2) {
//			player2WanderTarget = wanderTarget;
//		} else if (playerNumber == 3) {
//			player3WanderTarget = wanderTarget;
//		} else if (playerNumber == 4) {
//			player4WanderTarget = wanderTarget;
//		}
//
//
//		// project in front of agent
//		wanderTarget.z += wanderDistance;
//
//		return wanderTarget.normalized;
//	}

	// wall avoidance direction vector, local space, normalised
	Vector3 wallAvoidance(GameObject self) {
		// Feeler intersections
		RaycastHit hit;

		// default values for no hits on feelers
		float minDistance = Mathf.Infinity;
		RaycastHit minHit = new RaycastHit();
		bool hitWall = false;

		// forward feeler
		if (Physics.Raycast(self.transform.position, self.transform.forward, out hit, 10.0f)) {
			// if it's the shortest distance, it's the most important
			if (hit.distance < minDistance) {
				minDistance = hit.distance;
				minHit = hit;
				hitWall = true;
			}
		}
		// left feeler
		if (Physics.Raycast(self.transform.position, self.transform.forward - self.transform.right, out hit, 8.0f)) {
			// if it's the shortest distance, it's the most important
			if (hit.distance < minDistance) {
				minDistance = hit.distance;
				minHit = hit;
				hitWall = true;
			}
		}
		// right feeler
		if (Physics.Raycast(self.transform.position, self.transform.forward + self.transform.right, out hit, 8.0f)) {
			// if it's the shortest distance, it's the most important
			if (hit.distance < minDistance) {
				minDistance = hit.distance;
				minHit = hit;
				hitWall = true;
			}
		}

		// if a feeler hits a wall, return local space normal direction, else return a zero vector 
		if (hitWall) {
			return self.transform.InverseTransformDirection(minHit.normal);
		} else {
			return Vector3.zero;
		}
	}

	// wall avoidance direction vector, world space, normalised
	Vector3 OpenSpaceSeek(GameObject self) {
		RaycastHit hit;
		Vector3 rayOrigin = self.transform.position;
		rayOrigin.y += 1.4f;

		// default values for no hits on feelers
		float maxDistance = 0.0f;
		RaycastHit maxHit = new RaycastHit();
		bool hitWall = false;
		Vector3 avg = Vector3.zero;

		// forward feeler
		if (Physics.Raycast(rayOrigin, self.transform.forward, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// forward right feeler
		if (Physics.Raycast(rayOrigin, self.transform.forward + self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// right feeler
		if (Physics.Raycast(rayOrigin, self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// back right feeler
		if (Physics.Raycast(rayOrigin, -self.transform.forward + self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// back feeler
		if (Physics.Raycast(rayOrigin, -self.transform.forward, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// back left feeler
		if (Physics.Raycast(rayOrigin, -self.transform.forward + -self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// left feeler
		if (Physics.Raycast(rayOrigin, -self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}
		// forward left feeler
		if (Physics.Raycast(rayOrigin, self.transform.forward + -self.transform.right, out hit)) {
			// if it's the shortest distance, it's the most important
			avg += hit.point;
			if (hit.distance > maxDistance) {
				maxDistance = hit.distance;
				maxHit = hit;
				hitWall = true;
			}
		}

		avg = avg / 8.0f;

		// if a feeler hits a wall, return local space normal direction, else return a zero vector 
		if (hitWall) {
			return seek(self, avg);
		} else {
			return Vector3.zero;
		}
	}
}
