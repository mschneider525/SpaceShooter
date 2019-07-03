using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour
{
    public bool gameOver = false;
    public string gameMode = "";
    public bool gamePaused = false;

    [SerializeField]
    private GameObject _player = null;
    [SerializeField]
    private GameObject _player1 = null;
    [SerializeField]
    private GameObject _player2 = null;

    [SerializeField]
    private GameObject _pauseMenuPanel = null;

    public Player playerScript = null;
    public Player playerOneScript = null;
    public Player playerTwoScript = null;

    private UI_Manager _uiManager = null;
    private SpawnManager _spawnManager = null;
    private AudioSource _backgroundMusic = null;

    [SerializeField]
    private EventSystem _pauseMenuEventSystem = null;

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
            if ((Input.GetButtonDown("Cross") || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && _uiManager.instructionsMiddle.text != "")
            {
                StartGame();
            }
            if ((Input.GetButtonDown("Circle") || Input.GetKeyDown(KeyCode.M)) && _uiManager.instructionsTop.text != "")
            {
                EndGame();
            }
        }
        else
        {
            if (gamePaused == false && (Input.GetButtonDown("TouchPad") || Input.GetKeyDown(KeyCode.P)))
            {
                PauseGame();
            }
            else if (gamePaused == true && (Input.GetButtonDown("TouchPad") || Input.GetButtonDown("Circle") || Input.GetKeyDown(KeyCode.P)))
            {
                ResumeGame();
            }
            else
            {

            }

            UpdateInstructionText(gameMode);

            CheckIfPlayerLivesIsZeroAndInitiateGameOverIfTrue(gameMode);
            
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
        _backgroundMusic.Stop();
        _pauseMenuPanel.SetActive(true);
        HighlightFirstSelectedGameObject();
        gamePaused = true;
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        _backgroundMusic.Play();
        _pauseMenuPanel.SetActive(false);
        gamePaused = false;
        if (gameMode == "SinglePlayer")
        {
            if (playerScript.hasSpeedBoost == true)
            {
                SetTimeScaleAndFixedDeltaTime(0.7f);
            }
            else
            {
                SetTimeScaleAndFixedDeltaTime(1.0f);
            }
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            if (playerOneScript.hasSpeedBoost == true || playerTwoScript.hasSpeedBoost == true)
            {
                SetTimeScaleAndFixedDeltaTime(0.7f);
            }
            else
            {
                SetTimeScaleAndFixedDeltaTime(1.0f);
            }
        }
    }

    public void GameOver()
    {
        gameOver = true;
        _backgroundMusic.Stop();
        _uiManager.GameOver(gameMode);
    }

    public void EndGame()
    {
        SceneManager.LoadScene("MainMenu");
        gamePaused = false;
        SetTimeScaleAndFixedDeltaTime(1.0f);
    }

    public void SpawnPlayer(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            Instantiate(_player);
            playerScript = GameObject.Find("Player(Clone)").GetComponent<Player>();
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            Instantiate(_player1);
            playerOneScript = GameObject.Find("Player1(Clone)").GetComponent<Player>();
            Instantiate(_player2);
            playerTwoScript = GameObject.Find("Player2(Clone)").GetComponent<Player>();
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

        GameObject[] asteroidGameObjects = GameObject.FindGameObjectsWithTag("Asteroid");
        int numberOfAsteroids = asteroidGameObjects.Length;
        if (numberOfAsteroids > 0)
        {
            foreach (GameObject asteroid in asteroidGameObjects)
            {
                Destroy(asteroid);
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

    private void HighlightFirstSelectedGameObject()
    {
        _pauseMenuEventSystem.SetSelectedGameObject(null);
        //_pauseMenuEventSystem.SetSelectedGameObject(_pauseMenuEventSystem.firstSelectedGameObject);
    }

    private void UpdateInstructionText(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            if (_uiManager.instructionsTop.text != "" && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f || Input.GetAxis("Horizontal2") != 0.0f || Input.GetAxis("Vertical2") != 0.0f))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Top", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "W", "<sprite=\"PC_Outlined\" name=\"W\">"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "O", "<sprite=\"PC_Outlined\" name=\"O\">"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Top", "<sprite=\"PC_Outlined\" name=\"A\"><sprite=\"PC_Outlined\" name=\"S\"><sprite=\"PC_Outlined\" name=\"D\"> or <sprite=\"PC_Outlined\" name=\"K\"><sprite=\"PC_Outlined\" name=\"L\"><sprite=\"PC_Outlined\" name=\"SemiColon\"> to move"));
            }
            if (_uiManager.instructionsMiddle.text != "" && (Input.GetButtonDown("R2") || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Middle", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "Middle", "<sprite=\"PC_Outlined\" name=\"SpaceBar\"> or <sprite=\"PC_Outlined\" name=\"MouseLeft\"> to shoot"));
            }

            if (playerScript.playerLives == 0)
            {
                GameOver();
            }
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            //Player1
            if (_uiManager.instructionsTopLeft.text != "" && (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopLeft", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "W", "<sprite=\"PC_Outlined\" name=\"W\">"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopLeft", "<sprite=\"PC_Outlined\" name=\"A\"><sprite=\"PC_Outlined\" name=\"S\"><sprite=\"PC_Outlined\" name=\"D\"> to move"));
            }
            if (_uiManager.instructionsMiddleLeft.text != "" && (Input.GetButtonDown("L2") || Input.GetKeyDown(KeyCode.Space)))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleLeft", "<sprite=\"PS4_Outlined\" name=\"L2\"> to shoot"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleLeft", "<sprite=\"PC_Outlined\" name=\"SpaceBar\">to shoot"));
            }

            //Player2
            if (_uiManager.instructionsTopRight.text != "" && (Input.GetAxis("Horizontal2") != 0.0f || Input.GetAxis("Vertical2") != 0.0f))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopRight", "<sprite=\"PS4_Outlined\" name=\"JS Right\"> to move"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "8", "<sprite=\"PC_Outlined\" name=\"8\">"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "TopRight", "<sprite=\"PC_Outlined\" name=\"4\"><sprite=\"PC_Outlined\" name=\"5\"><sprite=\"PC_Outlined\" name=\"6\"> to move"));
            }
            if (_uiManager.instructionsMiddleRight.text != "" && (Input.GetButtonDown("R2") || Input.GetKeyDown(KeyCode.Keypad0)))
            {
                //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleRight", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
                StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.5f, "MiddleRight", "<sprite=\"PC_Outlined\" name=\"0\"> to shoot"));
            }

            if (playerOneScript.playerLives == 0 && playerTwoScript.playerLives == 0)
            {
                GameOver();
            }
        }

        if (_uiManager.instructionsBottom.text != "" && (Input.GetButtonDown("TouchPad") || Input.GetKeyDown(KeyCode.P)))
        {
            //StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.1f, "Bottom", "<sprite=\"PS4_TouchPad\" name=\"PS4_TouchPad\"> to pause"));
            StartCoroutine(_uiManager.TimeLimitInstructions_Routine(0.1f, "Bottom", "<sprite=\"PC_Outlined\" name=\"P\"> to pause"));
        }
    }

    private void CheckIfPlayerLivesIsZeroAndInitiateGameOverIfTrue(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            if (playerScript.playerLives == 0)
            {
                GameOver();
            }
        }

        if (gameMode == "SinglePlayerCo-op")
        {
            if (playerOneScript.playerLives == 0 && playerTwoScript.playerLives == 0)
            {
                GameOver();
            }
        }
    }

    public void SetTimeScaleAndFixedDeltaTime(float timeScale)
    {
        Time.timeScale = timeScale;
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

}
