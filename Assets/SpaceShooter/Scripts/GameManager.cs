using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;

    public GameObject player = null;
    public GameObject player1 = null;
    public GameObject player2 = null;

    private UI_Manager _uiManager = null;
    private SpawnManager _spawnManager = null;
    private AudioSource _backgroundMusic = null;


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _backgroundMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        StartGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver == true)
        {
            if ((Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("Cross")) && _uiManager.instructionsMiddle.text != "")
            {
                StartGame();
            }
            if ((Input.GetKeyDown(KeyCode.M) || Input.GetButtonDown("Circle")) && _uiManager.instructionsTop.text != "")
            {
                EndGame();
            }
        }
        else
        {
            if (_uiManager.instructionsTop.text != "" && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
            {
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Top", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
            }
            if (_uiManager.instructionsMiddle.text != "" && (Input.GetButtonDown("R2") || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Middle", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
            }
        }
    }

    private void StartGame()
    {
        DestroyEverything();
        _uiManager.StartGame();
        gameOver = false;
        SpawnPlayer(GetGameMode());
        _spawnManager.spawnEnemy = true;
    }

    public void GameOver()
    {
        gameOver = true;
        _uiManager.GameOver();
    }

    public void EndGame()
    {
        _uiManager.EndGame();
    }

    public string GetGameMode()
    {
        string gameMode = "";

        Scene currentScene = SceneManager.GetActiveScene();
        gameMode = currentScene.name;

        return gameMode;
    }

    public void SpawnPlayer(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            Instantiate(player);
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            Instantiate(player1);
            Instantiate(player2);
        }
        _backgroundMusic.Play();
    }

    public void DestroyEverything()
    {
        GameObject[] playerGameObjects = GameObject.FindGameObjectsWithTag("Player");
        int numberOfPlayers = playerGameObjects.Length;
        if (numberOfPlayers > 0)
        {
            foreach (GameObject player in playerGameObjects)
            {
                Destroy(player);
            }
        }

        GameObject[] enemyGameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        int numberOfEnemies = enemyGameObjects.Length;
        if (numberOfEnemies > 0)
        {
            foreach (GameObject enemy in enemyGameObjects)
            {
                Destroy(enemy);
            }
        }

        GameObject[] powerUpGameObjects = GameObject.FindGameObjectsWithTag("PowerUp");
        int numberOfPowerUps = powerUpGameObjects.Length;
        if (numberOfPowerUps > 0)
        {
            foreach (GameObject powerUp in powerUpGameObjects)
            {
                Destroy(powerUp);
            }
        }
    }

    
}
