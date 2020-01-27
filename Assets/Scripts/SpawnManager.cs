using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public EnemyObject[] _enemyPrefab;
    public PowerUpObject[] _powerUpPrefab;
    public GameObject _bossPrefab;
    public int bossWave;
    
    [SerializeField]
    private GameObject _enemyContainer, _powerUpContainer;
    [SerializeField]
    private int _waveMultiplier = 2, _waveIncrement = 5;
    
    [SerializeField]
    private int _currentWave = 0;
    private int _waveMax;
    private int _currEnemyCount = 0;

    [SerializeField]
    private float _enemySpawnDelay = 3f;

    private bool _stopSpawning = false;
    private bool _bossWave = false;
    
    private UIManager _uiManager;
    private GameManager _gameManager;
    private Coroutine _powerUpRoutine;

    // Start is called before the first frame update
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
            Debug.LogError("UI Mananger not found by SpawnManager!");
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
            Debug.LogError("Game Mananger not found by SpawnManager!");
    }
    public void StartEnemyWave()
    {
        _currentWave++;
        _stopSpawning = false;
        StartCoroutine(_uiManager.ShowWave(_currentWave));
        _powerUpRoutine = StartCoroutine(SpawnPowerUp());

        if (_currentWave == bossWave)
        {
            if (_gameManager.GetGameOver() == false)
            {
                BossWave();
                _bossWave = true;
            }
        }
        else
        {
            StartCoroutine(SpawnEnemies());
        }
    }

    private void BossWave()
    {
        Instantiate(_bossPrefab, new Vector3(0f, 9f, 0f), Quaternion.identity);
    }

    IEnumerator CheckEnemiesRemaining()
    {
        while (_enemyContainer.transform.childCount > 0)
        {
            yield return new WaitForSeconds(.1f);
        }
        
        if (_powerUpRoutine != null) StopCoroutine(_powerUpRoutine);
        if (!_gameManager.GetGameOver()) StartEnemyWave();
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_enemySpawnDelay);
        _waveMax = (_currentWave * _waveMultiplier) + _waveIncrement;
        
        _currEnemyCount = 0;
        
        
        while (_stopSpawning == false)
        {
            int i = UnityEngine.Random.Range(0, 101);
            for (int j = 0; j < _enemyPrefab.Length; j++)
            {
                if (i >= _enemyPrefab[j].lowerProbability && i <= _enemyPrefab[j].upperProbability)
                {
                    var enemy = Instantiate(_enemyPrefab[j].enemyPrefab, new Vector3(UnityEngine.Random.Range(-9f, 9f), 7, 0), Quaternion.identity);
                    enemy.transform.SetParent(_enemyContainer.transform);
                    break;
                }
            }
            
            _currEnemyCount++;
            if (_currEnemyCount >= _waveMax) StopSpawning();
            yield return new WaitForSeconds(_enemySpawnDelay);
        }
        
        StartCoroutine(CheckEnemiesRemaining());
    }
    public void StopSpawning()
    {
        _stopSpawning = true;
    }
    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(_enemySpawnDelay);

        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 7f));
            if (_gameManager.GetGameOver()) break;

            // Weighted Random (Laser Bursh should come out rarely
            int i = UnityEngine.Random.Range(0, 101);

            for (int j = 0; j < _powerUpPrefab.Length; j++)
            {
                if (i >= _powerUpPrefab[j].lowerProbability && i <= _powerUpPrefab[j].upperProbability)
                {
                    var powerUp = Instantiate(_powerUpPrefab[j].powerUpPrefab, new Vector3(UnityEngine.Random.Range(-9f, 9f), 8.5f, 0), Quaternion.identity);
                    powerUp.transform.SetParent(_powerUpContainer.transform);
                    break;
                }
            }

            
        }
        
    }
    public bool IsBossWave()
    {
        return _bossWave;
    }
}
[System.Serializable]
public class EnemyObject
{
    public GameObject enemyPrefab;
    public int lowerProbability;
    public int upperProbability;
}
[System.Serializable]
public class PowerUpObject
{
    public GameObject powerUpPrefab;
    public int lowerProbability;
    public int upperProbability;
}