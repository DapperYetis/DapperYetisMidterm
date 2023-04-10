using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    [Range(1, 30)] [SerializeField] int _enemyCount;
    [SerializeField] List<EnemyAI> _enemyList;
    [SerializeField] List<EnemySpawnStats> _spawnStats;
    [SerializeField] int _spawnPointIndex; // should be _spawnPointNumber

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _enemyList = new();

        StartCoroutine(Spawner(_spawnStats[_spawnPointIndex]));
    }

    private IEnumerator Spawner(EnemySpawnStats stats)
    {
        yield return new WaitForSeconds(stats.spawnInterval);
        if (_enemyList.Count < _enemyCount)
        {
            GameObject spawned = Instantiate(stats.prefab, stats.spawnPosition.position + new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 5f), Random.Range(-3f, 3f)), Quaternion.identity);
        }
        StartCoroutine(Spawner(stats));
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
