using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : Destructible
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _asteroidHealth = 3;
    [SerializeField]
    private float _xPosition = 0.0f;
    [SerializeField]
    private float _yPosition = 6.25f;

    [SerializeField]
    private GameObject _explosionPrefab = null;

    private GameManager _gameManager = null;
    private UI_Manager _uiManager = null;
    private Player _playerScript = null;

    private SpriteRenderer _asteroidSpriteRenderer = null;

    private GameObject _player = null;
    private GameObject _player1 = null;
    private GameObject _player2 = null;

    // Start is called before the first frame update
    void Start()
    {
        _xPosition = Random.Range(-8.2f, 8.2f);
        this.transform.position = new Vector3(_xPosition, _yPosition, 0);

        _asteroidSpriteRenderer = this.GetComponent<SpriteRenderer>();

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

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Second if condition: do not allow laser to destroy asteroid off screen
        if ((other.tag == "Laser" || other.tag == "TripleShot") && other.transform.position.y < 4.4f)
        {
            if (other.tag == "Laser")
            {
                _asteroidHealth--;
                StartCoroutine(DamageColorChange_Routine(this.gameObject));
            }

            if (_asteroidHealth == 0 || other.tag == "TripleShot")
            {
                DestroyAsteriod();
            }

            Destroy(other.gameObject);
        }

        if (other.tag == "Player")
        {
            DestroyAsteriod();

            _playerScript = other.GetComponent<Player>();
            if (_playerScript != null)
            {
                _playerScript.PlayerDamage();
            }
        }
    }

    private void DestroyAsteriod()
    {
        Destroy(this.gameObject);
        _uiManager.UpdateScoreText(false, this.gameObject.tag);
        ObjectExplosion(_explosionPrefab);
    }
}
