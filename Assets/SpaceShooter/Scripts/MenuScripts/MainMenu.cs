using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject _singlePlayerCross = null;
    [SerializeField]
    private GameObject _singlePlayerButton = null;
    [SerializeField]
    private GameObject _singlePlayerCoOpCross = null;
    [SerializeField]
    private GameObject _singlePlayerCoOpButton = null;
    [SerializeField]
    private GameObject _clearHighScoresCross = null;
    [SerializeField]
    private GameObject _clearHighScoresButton = null;
    [SerializeField]
    private GameObject _quitGameCross = null;
    [SerializeField]
    private GameObject _quitGameButton = null;

    [SerializeField]
    private AudioClip _audioClip_HighScoresCleared = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == _singlePlayerButton)
        {
            _singlePlayerCross.SetActive(true);
            _singlePlayerCoOpCross.SetActive(false);
            _clearHighScoresCross.SetActive(false);
            _quitGameCross.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject == _singlePlayerCoOpButton)
        {
            _singlePlayerCross.SetActive(false);
            _singlePlayerCoOpCross.SetActive(true);
            _clearHighScoresCross.SetActive(false);
            _quitGameCross.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject == _clearHighScoresButton)
        {
            _singlePlayerCross.SetActive(false);
            _singlePlayerCoOpCross.SetActive(false);
            _clearHighScoresCross.SetActive(true);
            _quitGameCross.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject == _quitGameButton)
        {
            _singlePlayerCross.SetActive(false);
            _singlePlayerCoOpCross.SetActive(false);
            _clearHighScoresCross.SetActive(false);
            _quitGameCross.SetActive(true);
        }

    }

    public void LoadSinglePlayerGame()
    {
        SceneManager.LoadScene("SinglePlayer");
    }

    public void LoadSinglePlayerCoOpGame()
    {
        SceneManager.LoadScene("SinglePlayerCo-op");
    }

    public void ClearHighScores()
    {
        PlayerPrefs.DeleteKey("HighScore_SinglePlayer");
        PlayerPrefs.DeleteKey("HighScore_SinglePlayerCo-op");

        //Too loud, so I moved the audio position farther away from the main camera
        Vector3 audioPosition = Camera.main.transform.position;
        audioPosition.z = -25.0f;

        AudioSource.PlayClipAtPoint(_audioClip_HighScoresCleared, audioPosition);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
