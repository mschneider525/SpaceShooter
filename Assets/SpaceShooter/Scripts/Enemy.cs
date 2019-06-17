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
    private float _powerUp_DropRate = 0.10f;

    [SerializeField]
    private GameObject _explosionEnemyPrefab = null;

    private SpawnManager _spawnManager = null;
    private UI_Manager _uiManager = null;
    private Player _player = null;

    private SpriteRenderer _enemySpriteRenderer = null;

    // Start is called before the first frame update
    void Start()
    {
        _xPosition = Random.Range(-8.2f, 8.2f);
        this.transform.position = new Vector3(_xPosition, _yPosition, 0);

        _enemySpriteRenderer = this.GetComponent<SpriteRenderer>();
        _powerUp_DropRate = 0.10f;

        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();
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

            _player = other.GetComponent<Player>();
            if (_player != null)
            {
                _player.Damage();
            }
        }
    }

    private void DestroyEnemy()
    {
        Destroy(this.gameObject);
        _uiManager.UpdateScoreText();
        ObjectExplosion(_explosionEnemyPrefab);
        SpawnPowerUp();
    }

    private void SpawnPowerUp()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

        if (_spawnManager != null && Random.value <= _powerUp_DropRate)
        {
            _spawnManager.SpawnPowerUp(this.transform.position);

            /*--Attempted to delay spawning of the power up a bit after enemy desctruction. It didn't work.--*/
            //Vector3 spawnPosition = this.transform.position;
            //IEnumerator spawnPowerUp = _spawnManager.DelaySpawnPowerUp_Routine(2.0f, spawnPosition);
            //StartCoroutine(spawnPowerUp);
        }
    }

}
