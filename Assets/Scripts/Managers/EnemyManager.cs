using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    [Range(1, 30)][SerializeField] int _enemyCount;
    [SerializeField] List<EnemyAI> _enemyList;
    [SerializeField] List<EnemySpawnStats> _spawnPointList; // A list of spawn points
    int _spawnPointCount;

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _enemyList = new();

        StartCoroutine(Spawner(_spawnPointList[Random.Range(0, _spawnPointCount)]));
        //StartCoroutine(Spawner(_spawnPointList[_spawnPointCount]));
    }

    private IEnumerator Spawner(EnemySpawnStats spawnPoint)
    {
        yield return new WaitForSeconds(spawnPoint.spawnInterval);
        if (_enemyList.Count < _enemyCount)
        {
            GameObject spawned = Instantiate(spawnPoint.spawnPointPrefab, spawnPoint.spawnPosition.position + new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 5f), Random.Range(-3f, 3f)), Quaternion.identity);
        }
        StartCoroutine(Spawner(spawnPoint));
    }

    public void AddEnemyToList(EnemyAI enemy)
    {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemyFromList(EnemyAI enemy)
    {
        _enemyList.Remove(enemy);
    }
}
