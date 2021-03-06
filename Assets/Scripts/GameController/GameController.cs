﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    public GameObject playerShipPrefab;
    public GameObject enemyShipPrefab;

    public GameObject[] asteroidPrefabs;

    public Transform playerSpawnPoint;
    public Transform[] enemySpawnPoints;

    public int numberOfEnemies = 0;

    public int numberOfAsteroids = 0;

    public GameObject canvas;

    public GameObject playerShip;
    public List<AiNeutral> enemyShips = new List<AiNeutral>();

    public List<Rigidbody> asteroids = new List<Rigidbody>();

    public bool gameOver = false;

    public GameObject gameOverPanel;

    public GameObject pauseGamePanel;

    public StatusBar playerStatus;

    public GameObject scorePanel;



    void Awake()
    {
        InitializeGame();
    }

    void Update()
    {
        if (!gameOver)
        {
            if (playerShip == null)
            {
                GameOver("DEFEAT");
            }
            else if (enemyShips.Count == 0)
            {
                GameOver("VICTORY");
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                TogglePaused();
            }

            UpdateScorePanel();
        }
    }

    public void InitializeGame()
    {
        canvas = GameObject.Find("Canvas");

        InitializePlayer();
        InitializeEnemies();
        InitializeEnvironment();
    }

    void InitializePlayer()
    {
        playerShip = GameObject.Instantiate(playerShipPrefab, playerSpawnPoint.position, Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180)));
        playerStatus.SetTarget(playerShip);
    }

    void InitializeEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 spawnPosition = enemySpawnPoints[Random.Range(0, enemySpawnPoints.Length - 1)].position + new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

            enemyShips.Add(GameObject.Instantiate(enemyShipPrefab, spawnPosition, spawnRotation).GetComponent<AiNeutral>());
            enemyShips[i].SetTarget(playerShip);
            enemyShips[i].SetAction(ActionType.ATTACK_TARGET);
        }
    }

    void InitializeEnvironment()
    {
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            Vector3 spawnPosition = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f), Random.Range(-100.0f, 100.0f));
            Quaternion spawnRotation = Quaternion.Euler(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

            GameObject prefab = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];

            asteroids.Add(GameObject.Instantiate(prefab, spawnPosition, spawnRotation).GetComponent<Rigidbody>());
        }
    }

    public void GameOver(string bannerText)
    {
        if (gameOverPanel && !gameOverPanel.activeSelf)
        {
            gameOver = true;

            gameOverPanel.SetActive(true);

            gameOverPanel.GetComponentInChildren<Text>().text = bannerText;
        }
    }

    public void ShipDestroyed(AiNeutral destroyedAI)
    {
        enemyShips.Remove(destroyedAI);
    }

    public void NavigateMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void TogglePaused()
    {
        if (pauseGamePanel)
        {
            if (pauseGamePanel.activeSelf)
            {
                pauseGamePanel.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
                pauseGamePanel.SetActive(true);
            }
        }
        else
        {
            Debug.Log("pauseGamePanel does not exist");
        }
    }

    void UpdateScorePanel()
    {
        int enemiesLeft = numberOfEnemies - enemyShips.Count;
        string readout = "Destroyed: " + enemiesLeft.ToString() + "/" + numberOfEnemies.ToString();

        scorePanel.transform.GetComponentInChildren<Text>().text = readout;
    }

}
