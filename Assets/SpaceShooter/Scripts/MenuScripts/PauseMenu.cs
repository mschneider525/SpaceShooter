using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameManager _gameManager = null;

    [SerializeField]
    private GameObject _resumeCross = null;
    [SerializeField]
    private GameObject _resumeButton = null;
    [SerializeField]
    private GameObject _quitCross = null;
    [SerializeField]
    private GameObject _quitButton = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == _resumeButton)
        {
            _resumeCross.SetActive(true);
            _quitCross.SetActive(false);
        }
        if (EventSystem.current.currentSelectedGameObject == _quitButton)
        {
            _resumeCross.SetActive(false);
            _quitCross.SetActive(true);
        }
    }

    public void ResumeGame()
    {
        _gameManager.ResumeGame();
    }

    public void QuitGame()
    {
        _gameManager.EndGame();
    }
}
