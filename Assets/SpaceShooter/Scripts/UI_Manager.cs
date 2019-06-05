﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField]
    private GameObject _playerLivesGameObject = null;
    [SerializeField]
    private GameObject _shieldsTextGameObject = null;
    [SerializeField]
    private GameObject _shieldsDisplayGameObject = null;
    [SerializeField]
    private GameObject _tripleShotAmmoTextGameObject = null;
    [SerializeField]
    private GameObject _tripleShotAmmoDisplayGameObject = null;
    [SerializeField]
    private GameObject _scoreGameObject = null;

    public Image playerlivesImage = null;
    public Sprite[] playerLivesSprites = new Sprite[4];

    public Text shieldsDisplayText = null;
    public Text tripleShotAmmoDisplayText = null;

    public Text scoreText = null;
    public int score = 0;

    public TextMeshProUGUI instructionsTop = null;
    public TextMeshProUGUI instructionsMiddle = null;
    public TextMeshProUGUI instructionsBottom = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        Reset_HUD();

        _playerLivesGameObject.SetActive(true);
        _shieldsTextGameObject.SetActive(true);
        _shieldsDisplayGameObject.SetActive(true);
        _tripleShotAmmoTextGameObject.SetActive(true);
        _tripleShotAmmoDisplayGameObject.SetActive(true);
        _scoreGameObject.SetActive(true);

        /*-----For Keyboard and Mouse-----*/
        //instructionsTop.fontSize = 15;
        //instructionsMiddle.fontSize = 15;
        //instructionsBottom.fontSize = 15;

        StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Top", "<sprite=\"PS4_Outlined\" name=\"JS Left\"> to move"));
        //StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Top", "Use [W][A][S][D] or [Arrow Keys] to move"));
        StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Middle", "<sprite=\"PS4_Outlined\" name=\"R2\"> to shoot"));
        //StartCoroutine(TimeLimitInstructions_Routine(5.0f, "Middle", "Use [Left Mouse] or [Space Bar] to shoot"));
        UpdateInstructionText("Bottom", "");
    }

    public void GameOver()
    {
        StartCoroutine(GameOverInstructionText_Routine(2.0f));
    }
    
    public void UpdateLives(int currentLives)
    {
        playerlivesImage.sprite = playerLivesSprites[currentLives];
    }

    public void UpdateShieldsText(int shieldLevel)
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

    public void UpdateTripleShotAmmoText(int tripleShotAmmo)
    {
        string tripleShotLasers = "";

        for (int i = 0; i < tripleShotAmmo; i++)
        {
            tripleShotLasers += "|";
        }

        tripleShotAmmoDisplayText.text = tripleShotLasers;
    }

    public void UpdateScoreText(bool resetScore = false, bool hasTripleShot = false)
    {
        score += 100;

        if (resetScore == true)
        {
            score = 0;
        }

        scoreText.text = "Score: " + score;
    }

    public void UpdateInstructionText(string instructionTextLocation, string instructionText)
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
            case "Middle":
                instructionsMiddle.text = instructionText;
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
            case "Middle":
                instructionsMiddle.text = "";
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

        UpdateInstructionText("Top", "GAME OVER");
        UpdateInstructionText("Middle", "<sprite=\"PS4_Outlined\" name=\"Cross\"> to play again");
        //UpdateInstructionText("Middle", "Press [Enter] to play again");
        UpdateInstructionText("Bottom", "<sprite=\"PS4_Outlined\" name=\"Circle\"> to return to the Main Menu");
        //UpdateInstructionText("Bottom", "Press [M] to return to the main Menu");
    }

    public void Reset_HUD()
    {
        UpdateLives(3);
        UpdateShieldsText(0);
        UpdateTripleShotAmmoText(0);
        UpdateScoreText(true);
    }
}