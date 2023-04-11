using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    List<EnemyAI> _enemyList;

    List<EnemySpawnStats> _spawnPointList;
    Dictionary<EnemySpawnStats, List<EnemyAI>> _enemiesBySpawn;

    [HideInInspector]
    public UnityEvent OnEnemyCountChange;

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
    }

    public void SetSpawnPoints(List<EnemySpawnStats> points)
    {
        _spawnPointList = points;
        _enemyList = new();
        _enemiesBySpawn = new();

        for (int i = 0; i < _spawnPointList.Count; i++)
        {
            _enemiesBySpawn[_spawnPointList[i]] = new();
            StartCoroutine(Spawner(_spawnPointList[i], 0));
        }
    }

    private IEnumerator Spawner(EnemySpawnStats spawnPoint, int spawnedCount)
    {
        yield return new WaitForSeconds(spawnPoint.spawnInterval);
        if (spawnedCount++ < spawnPoint.maxEnemyCount)
        {
            EnemyAI enemy = Instantiate(spawnPoint.spawnPointPrefab, spawnPoint.spawnPosition.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), Quaternion.identity).GetComponent<EnemyAI>();
            enemy.SetUp(spawnPoint);
            StartCoroutine(Spawner(spawnPoint, spawnedCount));
        }
    }

    public void AddEnemyToList(EnemyAI enemy, EnemySpawnStats spawnPoint)
    {
        _enemyList.Add(enemy);
        if (_enemiesBySpawn.ContainsKey(spawnPoint))
            _enemiesBySpawn[spawnPoint].Add(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy, EnemySpawnStats spawnPoint)
    {
        _enemyList.Remove(enemy);
        if(_enemiesBySpawn.ContainsKey(spawnPoint))
            _enemiesBySpawn[spawnPoint].Remove(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public int GetEnemyListSize()
    {
        return _enemyList.Count;
    }

    public void ResetMap()
    {
        foreach (EnemyAI enemy in _enemyList)
        {
            Destroy(enemy.gameObject);
        }
        _enemyList.Clear();
        _enemiesBySpawn.Clear();
        OnEnemyCountChange.RemoveAllListeners();
    }
}
