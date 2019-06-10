using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public string gameMode = "";

    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private GameObject _player1 = null;
    [SerializeField]
    private GameObject _player2 = null;

    [SerializeField]
    private GameObject _pauseMenuPanel = null;

    [SerializeField]
    private Player _playerScript = null;
    [SerializeField]
    private Player _playerOneScript = null;
    [SerializeField]
    private Player _playerTwoScript = null;

    private UI_Manager _uiManager = null;
    private SpawnManager _spawnManager = null;
    private AudioSource _backgroundMusic = null;


    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _backgroundMusic = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();

        gameMode = SceneManager.GetActiveScene().name;

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
            if (Input.GetButtonDown("Options"))
            {
                PauseGame();
            }

            if (gameMode == "SinglePlayer")
            {
                if (_uiManager.instructionsTop.text != "" && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Top", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
                }
                if (_uiManager.instructionsMiddle.text != "" && (Input.GetButtonDown("R2") || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Middle", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
                }

                if (_playerScript.playerLives == 0)
                {
                    GameOver();
                }
            }
            if (gameMode == "SinglePlayerCo-op")
            {
                if (_uiManager.instructionsTopLeft.text != "" && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopLeft", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
                }
                if (_uiManager.instructionsMiddleLeft.text != "" && Input.GetButtonDown("L2"))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleLeft", "<sprite=\"PS4_Outlined\" name=\"L2\"> to shoot"));
                }
                if (_uiManager.instructionsTopRight.text != "" && (Input.GetAxis("Horizontal2") != 0.0f || Input.GetAxis("Vertical2") != 0.0f))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopRight", "<sprite=\"PS4_Outlined\" name=\"JS Right\"> to move"));
                }
                if (_uiManager.instructionsMiddleRight.text != "" && Input.GetButtonDown("R2"))
                {
                    StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleRight", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
                }

                if (_playerOneScript.playerLives == 0 && _playerTwoScript.playerLives == 0)
                {
                    GameOver();
                }
            }
        }
    }

    private void StartGame()
    {
        DestroyEverything();
        SpawnPlayer(gameMode);
        gameOver = false;
        _uiManager.StartGame(gameMode);
        _spawnManager.spawnEnemy = true;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.0f;
        _pauseMenuPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        _pauseMenuPanel.SetActive(false);
    }

    public void GameOver()
    {
        gameOver = true;
        _backgroundMusic.Stop();
        _uiManager.GameOver();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void SpawnPlayer(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            Instantiate(_player);
            _playerScript = GameObject.Find("Player(Clone)").GetComponent<Player>();
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            Instantiate(_player1);
            _playerOneScript = GameObject.Find("Player1(Clone)").GetComponent<Player>();
            Instantiate(_player2);
            _playerTwoScript = GameObject.Find("Player2(Clone)").GetComponent<Player>();
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
