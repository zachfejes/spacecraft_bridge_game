using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject playerShipPrefab;
    public GameObject enemyShipPrefab;
    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    public int numberOfEnemies = 0;

    public GameObject playerShip;
    public List<AiNeutral> enemyShips = new List<AiNeutral>();


    public GameObject gameOverPanel;


    void Awake()
    {
        InitializeGame();
    }

    void Update()
    {
        if (playerShip == null)
        {
			GameOver("DEFEAT");
        }
        else if (enemyShips.Count == 0)
        {
			GameOver("VICTORY");
        }

    }

    public void InitializeGame()
    {
        playerShip = GameObject.Instantiate(playerShipPrefab, playerSpawnPoint.position, Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));

        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length - 1)].position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

            enemyShips.Add(GameObject.Instantiate(enemyShipPrefab, spawnPosition, spawnRotation).GetComponent<AiNeutral>());
            enemyShips[i].SetTarget(playerShip);
            enemyShips[i].SetAction(ActionType.ATTACK_TARGET);
        }
    }

    public void GameOver(string bannerText){
        if (gameOverPanel && !gameOverPanel.activeSelf)
        {
            gameOverPanel.SetActive(true);
            Transform gameOverHeader = gameOverPanel.transform.Find("CopyPanel");
            if (gameOverHeader)
            {
                gameOverHeader.GetComponentInChildren<Text>().text = bannerText;

            }

        }
    }

	public void ShipDestroyed(AiNeutral destroyedAI) {
		enemyShips.Remove(destroyedAI);
	}

	public void NavigateMainMenu() {
		SceneManager.LoadScene("Menu");
	}

	public void RestartGame() {
		Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
	}

}
