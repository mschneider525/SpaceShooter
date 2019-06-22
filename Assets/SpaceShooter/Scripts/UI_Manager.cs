using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Image playerLivesImage = null;
    public Sprite[] playerLivesSprites = new Sprite[4];

    public Image playerTwoLivesImage = null;
    public Sprite[] playerTwoLivesSprites = new Sprite[4];

    public Text shieldsDisplayText = null;
    public Text tripleShotAmmoDisplayText = null;
    public Text speedBoostDisplayText = null;

    public Text shieldsDisplayTwoText = null;
    public Text tripleShotAmmoDisplayTwoText = null;
    public Text speedBoostDisplayTwoText = null;

    public Text scoreText = null;
    public int score = 0;

    [SerializeField]
    private GameObject _explosionScorePrefab = null;
    private float _clearScoreTextTime = 0.0f;

    public Text highScoreText = null;
    public int highScore = 0;

    public TextMeshProUGUI instructionsTop = null;
    public TextMeshProUGUI instructionsMiddle = null;
    public TextMeshProUGUI instructionsBottom = null;

    public TextMeshProUGUI instructionsTopLeft = null;
    public TextMeshProUGUI instructionsMiddleLeft = null;

    public TextMeshProUGUI instructionsTopRight = null;
    public TextMeshProUGUI instructionsMiddleRight = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _clearScoreTextTime && _clearScoreTextTime != 0.0f)
        {
            scoreText.text = "";
            _clearScoreTextTime = 0.0f;
        }
    }

    public void StartGame(string gameMode)
    {
        Reset_HUD(gameMode);

        /*-----For Keyboard and Mouse-----*/
        //instructionsTop.fontSize = 15;
        //instructionsMiddle.fontSize = 15;
        //instructionsBottom.fontSize = 15;

        if (gameMode == "SinglePlayer")
        {
            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Top", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
            //StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Top", "Use [W][A][S][D] or [Arrow Keys] to move"));
            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Middle", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
            //StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Middle", "Use [Left Mouse] or [Space Bar] to shoot"));

        }
        //SinglePlayerCo-op Instructions
        if (gameMode == "SinglePlayerCo-op")
        {
            UpdateInstructionText("Top", "");
            UpdateInstructionText("Middle", "");

            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "TopLeft", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "MiddleLeft", "<sprite=\"PS4_Outlined\" name=\"L2\"> to shoot"));

            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "TopRight", "<sprite=\"PS4_Outlined\" name=\"JS Right\"> to move"));
            StartCoroutine(TimeLimitInstructions_Routine(5.0f, "MiddleRight", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
        }

        StartCoroutine(TimeLimitInstructions_Routine(3.0f, "Bottom", "<sprite=\"PS4_TouchPad\" name=\"PS4_TouchPad\"> to pause"));
    }

    public void GameOver(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            PlayerPrefs.SetInt("HighScore_SinglePlayer", highScore);
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            PlayerPrefs.SetInt("HighScore_SinglePlayerCo-op", highScore);
        }
        
        StartCoroutine(GameOverInstructionText_Routine(2.0f));
    }
    
    public void UpdateLives(string playerDesignation, int currentLives)
    {
        if (currentLives >= 0)
        {
            if (playerDesignation == "Player" || playerDesignation == "Player1")
            {
                playerLivesImage.sprite = playerLivesSprites[currentLives];
            }
            if (playerDesignation == "Player2")
            {
                playerTwoLivesImage.sprite = playerTwoLivesSprites[currentLives];
            } 
        }
    }

    public void UpdateShieldsText(string playerDesignation, int shieldLevel)
    {
        if (playerDesignation == "Player" || playerDesignation == "Player1")
        {
            switch (shieldLevel)
            {
                case 3:
                    shieldsDisplayText.color = new Color(0.3764706f, 0.7607843f, 0.9372549f, 1.0f); //Blue-ish
                    shieldsDisplayText.text = ")))";
                    break;
                case 2:
                    shieldsDisplayText.color = new Color(0.8972915f, 1.0f, 0.3632075f, 1.0f); //Yellow-ish
                    shieldsDisplayText.text = "))";
                    break;
                case 1:
                    shieldsDisplayText.color = new Color(1.0f, 0.2783019f, 0.2783019f, 1.0f); //Red-ish
                    shieldsDisplayText.text = ")";
                    break;
                case 0:
                    shieldsDisplayText.color = Color.white;
                    shieldsDisplayText.text = "";
                    break;
                default:

                    break;
            } 
        }
        if (playerDesignation == "Player2")
        {
            switch (shieldLevel)
            {
                case 3:
                    shieldsDisplayTwoText.color = new Color(0.3764706f, 0.7607843f, 0.9372549f, 1.0f); //Blue-ish
                    shieldsDisplayTwoText.text = ")))";
                    break;
                case 2:
                    shieldsDisplayTwoText.color = new Color(0.8972915f, 1.0f, 0.3632075f, 1.0f); //Yellow-ish
                    shieldsDisplayTwoText.text = "))";
                    break;
                case 1:
                    shieldsDisplayTwoText.color = new Color(1.0f, 0.2783019f, 0.2783019f, 1.0f); //Red-ish
                    shieldsDisplayTwoText.text = ")";
                    break;
                case 0:
                    shieldsDisplayTwoText.color = Color.white;
                    shieldsDisplayTwoText.text = "";
                    break;
                default:

                    break;
            }
        }
    }

    public void UpdateTripleShotAmmoText(string playerDesignation, int tripleShotAmmo)
    {
        string tripleShotLasers = "";

        for (int i = 0; i < tripleShotAmmo; i++)
        {
            tripleShotLasers += "|";
        }

        if (playerDesignation == "Player" || playerDesignation == "Player1")
        {
            tripleShotAmmoDisplayText.text = tripleShotLasers;
        }
        if (playerDesignation == "Player2")
        {
            tripleShotAmmoDisplayTwoText.text = tripleShotLasers;
        }
    }

    public void UpdateSpeedBoostText(string playerDesignation, float speedBoostEndTime)
    {
        float speedBoostActiveTime = 0.0f;
        string speedBoostActiveTimeText = "0.00";

        if (speedBoostEndTime > Time.time)
        {
            speedBoostActiveTime = speedBoostEndTime - Time.time;
            speedBoostActiveTimeText = speedBoostActiveTime.ToString("f2"); //float to 2 decimal places
        }

        if (playerDesignation == "Player" || playerDesignation == "Player1")
        {
            speedBoostDisplayText.text = speedBoostActiveTimeText + "s";
        }
        if (playerDesignation == "Player2")
        {
            speedBoostDisplayTwoText.text = speedBoostActiveTimeText + "s";
        }
        
    }

    public void UpdateScoreText(bool resetScore = false, string objectTag = null)
    {
        if (objectTag == "Asteroid")
            score += 100;
        else
            score += 500;

        if (resetScore == true)
        {
            score = 0;
            scoreText.text = "Score: " + score;
            return;
        }

        if (scoreText.text != "")
        {
            scoreText.text = "Score: " + score; 
        }

        UpdateHighScoreText();
    }

    public void UpdateHighScoreText(bool resetScore = false)
    {
        highScoreText.text = "High Score: " + highScore;

        if (resetScore == true)
        {
            return;
        }

        if (score > highScore)
        {
            highScore = score;

            highScoreText.text = "High Score: " + highScore;

            if (scoreText.text != "")
            {
                ScoreExplosion(); 
            }

            _clearScoreTextTime = Time.time + 0.5f;
        }
    }

    public void UpdateInstructionText(string instructionTextLocation, string instructionText)
    {
        switch (instructionTextLocation)
        {
            case "Top":
                instructionsTop.text = instructionText;
                if (instructionsTop.text == "GAME OVER" || instructionsTop.text.Contains("NEW HIGH SCORE"))
                    instructionsTop.fontSize = 24;
                else
                    instructionsTop.fontSize = 20;
                    //instructionsTop.fontSize = 15;
                break;
            case "Middle":
                instructionsMiddle.text = instructionText;
                break;
            case "Bottom":
                instructionsBottom.text = instructionText;
                break;
            default:
                break;
        }
    }

    public IEnumerator TimeLimitInstructions_Routine(float countDown, string instructionTextLocation, string instructionText)
    {
        switch (instructionTextLocation)
        {
            case "Top":
                instructionsTop.text = instructionText;
                if (instructionsTop.text == "GAME OVER")
                    instructionsTop.fontSize = 32;
                else
                    instructionsTop.fontSize = 20;
                    //instructionsTop.fontSize = 15;
                break;
            case "TopLeft":
                instructionsTopLeft.text = instructionText;
                break;
            case "TopRight":
                instructionsTopRight.text = instructionText;
                break;
            case "Middle":
                instructionsMiddle.text = instructionText;
                break;
            case "MiddleLeft":
                instructionsMiddleLeft.text = instructionText;
                break;
            case "MiddleRight":
                instructionsMiddleRight.text = instructionText;
                break;
            case "Bottom":
                instructionsBottom.text = instructionText;
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(countDown);

        switch (instructionTextLocation)
        {
            case "Top":
                instructionsTop.text = "";
                break;
            case "TopLeft":
                instructionsTopLeft.text = "";
                break;
            case "TopRight":
                instructionsTopRight.text = "";
                break;
            case "Middle":
                instructionsMiddle.text = "";
                break;
            case "MiddleLeft":
                instructionsMiddleLeft.text = "";
                break;
            case "MiddleRight":
                instructionsMiddleRight.text = "";
                break;
            case "Bottom":
                instructionsBottom.text = "";
                break;
            default:
                break;
        }

    }

    public IEnumerator GameOverInstructionText_Routine(float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);

        if (scoreText.text != "")
        {
            UpdateInstructionText("Top", "GAME OVER"); 
        }
        else
        {
            UpdateInstructionText("Top", "NEW HIGH SCORE: " + highScore);
        }

        UpdateInstructionText("Middle", "<sprite=\"PS4_Outlined\" name=\"Cross\"> to play again");
        //UpdateInstructionText("Middle", "Press [Enter] to play again");
        UpdateInstructionText("Bottom", "<sprite=\"PS4_Outlined\" name=\"Circle\"> to return to the Main Menu");
        //UpdateInstructionText("Bottom", "Press [M] to return to the main Menu");
    }

    public void Reset_HUD(string gameMode)
    {
        string playerDesignation = "";

        if (gameMode == "SinglePlayer")
        {
            playerDesignation = "Player";

            UpdateLives(playerDesignation, 3);
            UpdateShieldsText(playerDesignation, 0);
            UpdateTripleShotAmmoText(playerDesignation, 0);
            UpdateSpeedBoostText(playerDesignation, 0.0f);

            highScore = PlayerPrefs.GetInt("HighScore_SinglePlayer");

            if (highScore == 0)
            {
                highScore = 2000;
                PlayerPrefs.SetInt("HighScore_SinglePlayer", highScore);
            }
        }
        if (gameMode == "SinglePlayerCo-op")
        {
            playerDesignation = "Player1";

            UpdateLives(playerDesignation, 3);
            UpdateShieldsText(playerDesignation, 0);
            UpdateTripleShotAmmoText(playerDesignation, 0);
            UpdateSpeedBoostText(playerDesignation, 0.0f);

            playerDesignation = "Player2";

            UpdateLives(playerDesignation, 3);
            UpdateShieldsText(playerDesignation, 0);
            UpdateTripleShotAmmoText(playerDesignation, 0);
            UpdateSpeedBoostText(playerDesignation, 0.0f);

            highScore = PlayerPrefs.GetInt("HighScore_SinglePlayerCo-op");

            if (highScore == 0)
            {
                highScore = 4000;
                PlayerPrefs.SetInt("HighScore_SinglePlayerCo-op", highScore);
            }
        }

        UpdateScoreText(true);
        UpdateHighScoreText(true);
    }

    private void ScoreExplosion()
    {
        GameObject scoreExplosion0 = Instantiate(_explosionScorePrefab, new Vector3(-1.4f, 4.0f, 0.0f), Quaternion.identity);
        Destroy(scoreExplosion0, 2.5f);

        GameObject scoreExplosion1 = Instantiate(_explosionScorePrefab, new Vector3(-0.7f, 4.0f, 0.0f), Quaternion.identity);
        Destroy(scoreExplosion1, 2.5f);

        GameObject scoreExplosion2 = Instantiate(_explosionScorePrefab, new Vector3(0.0f, 4.0f, 0.0f), Quaternion.identity);
        Destroy(scoreExplosion2, 2.5f);

        GameObject scoreExplosion3 = Instantiate(_explosionScorePrefab, new Vector3(0.7f, 4.0f, 0.0f), Quaternion.identity);
        Destroy(scoreExplosion3, 2.5f);

        GameObject scoreExplosion4 = Instantiate(_explosionScorePrefab, new Vector3(1.4f, 4.0f, 0.0f), Quaternion.identity);
        Destroy(scoreExplosion4, 2.5f);
    }
    
}
