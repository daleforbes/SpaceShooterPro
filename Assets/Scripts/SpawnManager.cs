using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _powerUpPrefab;
    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private float _enemySpawnDelay = 2f;

    private bool _stopSpawning = false;

    // Start is called before the first frame update
    public void StartEnemyWave()
    {
        StartCoroutine(SpawnEnemies());
        StartCoroutine(SpawnPowerUp());
    }


    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            var enemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-9f, 9f), 7, 0), Quaternion.identity);
            enemy.transform.SetParent(_enemyContainer.transform);
            yield return new WaitForSeconds(_enemySpawnDelay);
        }

    }
    public void StopSpawning()
    {
        _stopSpawning = true;
    }
    IEnumerator SpawnPowerUp()
    {
        yield return new WaitForSeconds(2.0f);
        while (_stopSpawning == false)
        {
            yield return new WaitForSeconds(Random.Range(3f, 7f));

            int i = Random.Range(0, _powerUpPrefab.Length);
            Instantiate(_powerUpPrefab[i], new Vector3(Random.Range(-9f, 9f), 8.5f, 0), Quaternion.identity);
        }
    }
}
