using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyShipPrefab = null;
    [SerializeField]
    private float _enemySpawn_xPosition = 0.0f;
    [SerializeField]
    private float _enemySpawn_yPosition = 6.25f;

    [SerializeField]
    private GameObject _playerPrefab = null;
    [SerializeField]
    private GameObject[] _powerUps = null;

    public bool spawnEnemy = false;

    private UI_Manager _uiManager = null;
    private float _enemySpawnRate = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        _uiManager = GameObject.Find("UI").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {
        _playerPrefab = GameObject.FindGameObjectWithTag("Player");
        if (_playerPrefab != null)
        {
            Player player = _playerPrefab.GetComponent<Player>();
            if (player != null && spawnEnemy == true)
            {
                StartCoroutine(SpawnEnemy_Routine(player, _uiManager.score));
                spawnEnemy = false;
            }
        }
    }

    //Creat coroutine to spawn an enemy every five seconds
    public IEnumerator SpawnEnemy_Routine(Player player, int score)
    {
        GameObject[] enemyShipList = GameObject.FindGameObjectsWithTag("Enemy");

        if (player.playerLives > 0)
        {
            if (enemyShipList != null && enemyShipList.Length <= 50)
            {
                _enemySpawn_xPosition = Random.Range(-8.2f, 8.2f);
                _enemyShipPrefab.transform.position = new Vector3(_enemySpawn_xPosition, _enemySpawn_yPosition, 0);
                Instantiate(_enemyShipPrefab, _enemyShipPrefab.transform.position, Quaternion.identity);
            }

            if (score >= 10000)
            {
                _enemySpawnRate = 0.5f;
            }
            else if (score >= 9000)
            {
                _enemySpawnRate = 0.6f;
            }
            else if (score >= 8000)
            {
                _enemySpawnRate = 0.7f;
            }
            else if (score >= 7000)
            {
                _enemySpawnRate = 0.8f;
            }
            else if (score >= 6000)
            {
                _enemySpawnRate = 0.9f;
            }
            else if (score >= 5000)
            {
                _enemySpawnRate = 1.0f;
            }
            else if (score >= 4000)
            {
                _enemySpawnRate = 1.1f;
            }
            else if (score >= 3000)
            {
                _enemySpawnRate = 1.2f;
            }
            else if (score >= 2000)
            {
                _enemySpawnRate = 1.3f;
            }
            else if (score >= 1000)
            {
                _enemySpawnRate = 1.4f;
            }
            else
            {
                _enemySpawnRate = 1.5f;
            }

            yield return new WaitForSeconds(_enemySpawnRate);
            
            spawnEnemy = true;
        }
    }

    public void SpawnPowerUp(Vector3 position)
    {
        //0: TripleShot
        //1: SpeedBoost
        //2: Shield
        int randomPowerUp = Random.Range(0, 3);
        Instantiate(_powerUps[randomPowerUp], position, Quaternion.identity);
    }

    /*--Attempted to delay spawning of the power up a bit after enemy destruction. It didn't work.--*/
    //public IEnumerator DelaySpawnPowerUp_Routine(float delayTime, Vector3 position)
    //{
    //    yield return new WaitForSeconds(delayTime);

    //    //0: TripleShot
    //    //1: SpeedBoost
    //    //2: Shield
    //    int randomPowerUp = Random.Range(0, 3);
    //    Instantiate(_powerUps[randomPowerUp], position, Quaternion.identity);
    //}
}
