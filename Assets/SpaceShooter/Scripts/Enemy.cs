using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Destructible
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private int _enemyHealth = 3;
    [SerializeField]
    private float _xPosition = 0.0f;
    [SerializeField]
    private float _yPosition = 6.25f;

    [SerializeField]
    private GameObject _explosionEnemyPrefab = null;

    private GameManager _gameManager = null;
    private SpawnManager _spawnManager = null;
    private UI_Manager _uiManager = null;
    private Player _playerScript = null;

    private SpriteRenderer _enemySpriteRenderer = null;

    [SerializeField]
    private GameObject _laserEnemyPrefab = null;

    [SerializeField]
    private float _coolDownTime = 1.5f;
    private float _nextFireTime = 0.0f;

    private GameObject _player = null;
    private GameObject _player1 = null;
    private GameObject _player2 = null;

    // Start is called before the first frame update
    void Start()
    {
        _xPosition = Random.Range(-8.2f, 8.2f);
        this.transform.position = new Vector3(_xPosition, _yPosition, 0);

        _enemySpriteRenderer = this.GetComponent<SpriteRenderer>();

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();

        if (_gameManager.gameMode == "SinglePlayer")
        {
            _player = GameObject.Find("Player(Clone)");
        }
        if (_gameManager.gameMode == "SinglePlayerCo-op")
        {
            _player1 = GameObject.Find("Player1(Clone)");
            _player2 = GameObject.Find("Player2(Clone)");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        Shoot();
    }

    private void Movement()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (this.transform.position.y < -6.25)
        {
            _xPosition = Random.Range(-8.2f, 8.2f);
            this.transform.position = new Vector3(_xPosition, _yPosition, 0);
        }
    }

    private void Shoot()
    {
        //if player's x position is between -0.3 and 0.3 of enemy's x position, shoot
        if (_gameManager.gameMode == "SinglePlayer")
        {
            if (_player != null)
            {
                if (_player.transform.position.x > (this.transform.position.x - 0.4f) && _player.transform.position.x < (this.transform.position.x + 0.4f))
                {
                    if (Time.time > _nextFireTime)
                    {
                        Enemy_Shoot();
                    }
                } 
            }
        }
        if (_gameManager.gameMode == "SinglePlayerCo-op")
        {
            if (_player1 != null)
            {
                if (_player1.transform.position.x > (this.transform.position.x - 0.4f) && _player1.transform.position.x < (this.transform.position.x + 0.4f))
                {
                    if (Time.time > _nextFireTime)
                    {
                        Enemy_Shoot();
                    }
                }
            }
            if (_player2 != null)
            {
                if (_player2.transform.position.x > (this.transform.position.x - 0.4f) && _player2.transform.position.x < (this.transform.position.x + 0.4f))
                {
                    if (Time.time > _nextFireTime)
                    {
                        Enemy_Shoot();
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Second if condition: do not allow laser to destroy enemy off screen
        if ((other.tag == "Laser" || other.tag == "TripleShot") && other.transform.position.y < 4.4f)
        {
            if (other.tag == "Laser")
            {
                _enemyHealth--;
                StartCoroutine(DamageColorChange_Routine(this.gameObject));
            }

            if (_enemyHealth == 0 || other.tag == "TripleShot")
            {
                DestroyEnemy();
            }

            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            DestroyEnemy();

            _playerScript = other.GetComponent<Player>();
            if (_playerScript != null)
            {
                _playerScript.Damage();
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
        _uiManager.UpdateScoreText(false, this.gameObject.tag);
        ObjectExplosion(_explosionEnemyPrefab);
        SpawnPowerUp();
    }

    private void SpawnPowerUp()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager != null)
        {
            _spawnManager.SpawnPowerUp(this.transform.position);
        }
    }

    private void Enemy_Shoot()
    {
        //Prevent the first shots from firing immediately; fire after 0.5 seconds
        if (_nextFireTime == 0.0f)
        {
            _nextFireTime = Time.time + 0.5f;
            return;
        }

        Instantiate(_laserEnemyPrefab, this.transform.position, Quaternion.identity);
        _nextFireTime = Time.time + _coolDownTime;
    }
}
