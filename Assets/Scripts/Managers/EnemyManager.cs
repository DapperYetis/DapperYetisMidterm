using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    [Range(1, 30)][SerializeField] int _maxEnemyCount;
    [SerializeField] List<EnemyAI> _enemyList;
    //[SerializeField] List<GameObject> _allEnemyTypes;
    [SerializeField] List<EnemySpawnStats> _spawnPointList; // A list of spawn points
    int _spawnPointCount;

    public UnityEvent OnEnemyCountChange;

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _enemyList = new();



        for (int i = 0; i < _spawnPointList.Count; i++)
        {
            StartCoroutine(Spawner(_spawnPointList[i]));
        }
    }

    private IEnumerator Spawner(EnemySpawnStats spawnPoint)
    {
        yield return new WaitForSeconds(spawnPoint.spawnInterval);
        if (_enemyList.Count < _maxEnemyCount)
        {
            GameObject spawned = Instantiate(spawnPoint.spawnPointPrefab, spawnPoint.spawnPosition.position + new Vector3(Random.Range(-3f, 3f), Random.Range(0f, 5f), Random.Range(-3f, 3f)), Quaternion.identity);
        }
        StartCoroutine(Spawner(spawnPoint));
    }

    public void AddEnemyToList(EnemyAI enemy)
    {
        _enemyList.Add(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy)
    {
        _enemyList.Remove(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public int GetEnemyListSize()
    {
        return _enemyList.Count;
    }
}
