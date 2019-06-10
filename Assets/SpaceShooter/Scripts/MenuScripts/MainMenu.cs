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
        }
        if (EventSystem.current.currentSelectedGameObject == _singlePlayerCoOpButton)
        {
            _singlePlayerCross.SetActive(false);
            _singlePlayerCoOpCross.SetActive(true);
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
}
