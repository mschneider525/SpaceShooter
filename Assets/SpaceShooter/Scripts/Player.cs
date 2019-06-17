using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Destructible
{
    public int playerLives = 3;
    public int tripleShotAmmo = 0;
    public bool hasSpeedBoost = false;
    public int shieldLevel = 0;

    public string playerDesignation = "";

    [SerializeField]
    private float _speed = 8.0f;
    [SerializeField]
    private float _upperPlayerBoundary= 4.2f;
    [SerializeField]
    private float _lowerPlayerBoundary = -4.2f;
    [SerializeField]
    private float _rightPlayerBoundary = 8.2f;
    [SerializeField]
    private float _leftPlayerBoundary = -8.2f;

    [SerializeField]
    private GameObject _laserPrefab = null;
    [SerializeField]
    private GameObject _tripleShotPrefab = null;
    [SerializeField]
    private GameObject _explosionPlayerPrefab = null;
    [SerializeField]
    private GameObject _shield = null;
    [SerializeField]
    private GameObject _thruster = null;
    [SerializeField]
    private GameObject[] _engineFires = new GameObject[2];

    [SerializeField]
    private float _coolDownTime = 0.20f;
    private float _nextFireTime = 0.0f;

    [SerializeField]
    private float _speedBoostActiveTime = 10.0f;
    [SerializeField]
    private float _speedBoostEndTime = 0.0f;

    private GameManager _gameManager = null;
    private UI_Manager _uiManager = null;


    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();

        SetPlayerStartingPositionAndDesignation(_gameManager.gameMode);

        if (_uiManager != null)
        {
            _uiManager.UpdateLives(playerDesignation, playerLives);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > _speedBoostEndTime)
        {
            hasSpeedBoost = false;

            if (_gameManager.gamePaused == false)
            {
                if (_gameManager.gameMode == "SinglePlayer")
                {
                    _gameManager.SetTimeScaleAndFixedDeltaTime(1.0f);
                }
                if (_gameManager.gameMode == "SinglePlayerCo-op")
                {
                    if (_gameManager.playerOneScript.hasSpeedBoost == false && _gameManager.playerTwoScript.hasSpeedBoost == false)
                    {
                        _gameManager.SetTimeScaleAndFixedDeltaTime(1.0f);
                    }
                } 
            }
        }

        ThrusterDisplay(hasSpeedBoost);
        _uiManager.UpdateSpeedBoostText(playerDesignation, _speedBoostEndTime);

        InvokePlayerInput(playerDesignation);
    }

    private void InvokePlayerInput(string playerDesignation)
    {
        float horizontalInput = 0.0f;
        float verticalInput = 0.0f;

        if (playerDesignation == "Player1")
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (Input.GetButton("L2"))
            {
                Shoot();
            }
        }
        else if (playerDesignation == "Player2")
        {
            horizontalInput = Input.GetAxis("Horizontal2");
            verticalInput = Input.GetAxis("Vertical2");

            if (Input.GetButton("R2"))
            {
                Shoot();
            }
        }
        else //if(playerDesignation == "Player")
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");

            if (Input.GetButton("R2"))
            {
                Shoot();
            }
        }

        Movement(horizontalInput, verticalInput);
    }

    private void Movement(float horizontalInput, float verticalInput)
    {
        if (hasSpeedBoost == true)
        {
            _speed = 12.0f;
        }
        else
        {
            _speed = 8.0f;
        }

        this.transform.Translate(Vector3.right * _speed * horizontalInput * Time.deltaTime);
        this.transform.Translate(Vector3.up * _speed * verticalInput * Time.deltaTime);

        //y-axis player boundary
        if (this.transform.position.y > _upperPlayerBoundary)
        {
            this.transform.position = new Vector3(this.transform.position.x, _upperPlayerBoundary, 0);
        }
        if (this.transform.position.y < _lowerPlayerBoundary)
        {
            this.transform.position = new Vector3(this.transform.position.x, _lowerPlayerBoundary, 0);
        }

        //x-axis player boundary
        if (this.transform.position.x > _rightPlayerBoundary)
        {
            this.transform.position = new Vector3(_rightPlayerBoundary, this.transform.position.y, 0);
        }
        if (this.transform.position.x < _leftPlayerBoundary)
        {
            this.transform.position = new Vector3(_leftPlayerBoundary, this.transform.position.y, 0);
        }

        /*-----x-axis player boundary wrapping-----*/
        #region x-axis player boundary wrapping
        //if (this.transform.position.x > 9.5f)
        //{
        //    this.transform.position = new Vector3(-8.8f, this.transform.position.y, 0);
        //}
        //if (this.transform.position.x < -9.5f)
        //{
        //    this.transform.position = new Vector3(8.8f, this.transform.position.y, 0);
        //}
        #endregion
        /*-----x-axis player boundary wrapping-----*/
    }

    private void Shoot()
    {
        if (Time.time > _nextFireTime)
        {
            if (tripleShotAmmo > 0)
            {
                Instantiate(_tripleShotPrefab, this.transform.position, Quaternion.identity);
                tripleShotAmmo = tripleShotAmmo - 1;
                _uiManager.UpdateTripleShotAmmoText(playerDesignation, tripleShotAmmo);
            }
            else
            {
                Instantiate(_laserPrefab, this.transform.position + new Vector3(0, 0.715f, 0), Quaternion.identity);
            }

            _nextFireTime = Time.time + _coolDownTime;
        }
    }

    public void Damage()
    {
        if (shieldLevel > 0)
        {
            shieldLevel = shieldLevel - 1;
            
            StartCoroutine(DamageColorChange_Routine(_shield));
            _uiManager.UpdateShieldsText(playerDesignation, shieldLevel);

            if (shieldLevel == 0)
            {
                _shield.SetActive(false);
            }

            return;
        }

        playerLives = playerLives - 1;
        StartCoroutine(DamageColorChange_Routine(this.gameObject));
        _uiManager.UpdateLives(playerDesignation, playerLives);

        ShowEngineFires(playerLives);

        if (playerLives == 0)
        {
            tripleShotAmmo = 0;
            _uiManager.UpdateTripleShotAmmoText(playerDesignation, tripleShotAmmo);
            hasSpeedBoost = false;
            _uiManager.UpdateSpeedBoostText(playerDesignation, 0.0f);
            Destroy(this.gameObject);
            ObjectExplosion(_explosionPlayerPrefab);
        }
    }

    public void Toggle_PowerUp(string powerUp)
    {
        switch (powerUp)
        {
            case "TripleShot":
                tripleShotAmmo = 15;
                _uiManager.UpdateTripleShotAmmoText(playerDesignation, tripleShotAmmo);
                break;
            case "SpeedBoost":
                hasSpeedBoost = true;
                _speedBoostEndTime = Time.time + _speedBoostActiveTime;
                _gameManager.SetTimeScaleAndFixedDeltaTime(0.7f);
                break;
            case "Shield":
                shieldLevel = 3;
                _shield.SetActive(true);
                _uiManager.UpdateShieldsText(playerDesignation, shieldLevel);
                break;
            default:

                break;
        }
    }

    public void ThrusterDisplay(bool hasSpeedBoost)
    {
        float yPosition = -2.44f;
        float yScale = 0.5f;

        if (hasSpeedBoost == true)
        {
            yPosition = -3.3f;
            yScale = 1.0f;
        }
        else
        {
            yPosition = -2.5f;
            yScale = 0.5f;
        }

        _thruster.transform.localPosition = new Vector3(0.0f, yPosition, 0.0f);
        _thruster.transform.localScale = new Vector3(0.33f, yScale, 0.5f);
    }

    public void ShowEngineFires(int playerLives)
    {
        //_enginFires[0] = EngineFireLeft
        //_enginFires[1] = EngineFireRight

        int randomInt = Random.Range(0, 2);

        if (playerLives > 0 && playerLives < 3)
        {
            if (_engineFires[0].activeSelf == false && _engineFires[1].activeSelf == false)
            {
                _engineFires[randomInt].SetActive(true);
            }
            else if (_engineFires[0].activeSelf == false)
            {
                _engineFires[0].SetActive(true);
            }
            else //if (_engineFires[1].activeSelf == false)
            {
                _engineFires[1].SetActive(true);
            }

        }
    }

    private void SetPlayerStartingPositionAndDesignation(string gameMode)
    {
        if (gameMode == "SinglePlayer")
        {
            this.transform.position = new Vector3(0, -3.0f, 0);
            playerDesignation = "Player";
        }

        if (gameMode == "SinglePlayerCo-op")
        {
            if (this.transform.name == "Player1(Clone)")
            {
                this.transform.position = new Vector3(-3.0f, -3.0f, 0);
                playerDesignation = "Player1";
            }
            if (this.transform.name == "Player2(Clone)")
            {
                this.transform.position = new Vector3(3.0f, -3.0f, 0);
                playerDesignation = "Player2";
            }
        }
    }

}
