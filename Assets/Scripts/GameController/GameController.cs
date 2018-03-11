using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public GameObject playerShipPrefab;
	public GameObject enemyShipPrefab;

	public GameObject spacecraftParent;
	public Transform playerSpawnPoint;
	public Transform[] enemySpawnPoints;

	public int numberOfEnemies = 0;

	public GameObject playerShip;
	public List<GameObject> enemyShips = new List<GameObject>();


	void Awake() {
		InitializeGame();
	}

	public void InitializeGame() {
		playerShip = GameObject.Instantiate(playerShipPrefab, playerSpawnPoint.position, Quaternion.Euler(Random.Range(-180,180), Random.Range(-180,180),Random.Range(-180,180)));
		
		for (int i = 0; i < numberOfEnemies; i++) {
			Vector3 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length - 1)].position + new Vector3(Random.Range(-10, 10), Random.Range(-10,10), Random.Range(-10,10));
			Quaternion spawnRotation = Quaternion.Euler(Random.Range(-180,180), Random.Range(-180,180),Random.Range(-180,180));

			enemyShips.Add(GameObject.Instantiate(enemyShipPrefab, spawnPosition, spawnRotation));
		}
	}


}
