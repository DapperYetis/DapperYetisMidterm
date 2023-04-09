using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    [SerializeField] List<EnemyAI> _enemyList;

    [SerializeField] List<EnemySpawnStats> _spawnStats;

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _enemyList = new();

        StartCoroutine(Spawner(_spawnStats[0].spawnInterval, _spawnStats[0].prefab));
    }

    private IEnumerator Spawner(float timer, GameObject enemy)
    {
        yield return new WaitForSeconds(timer);
        if (_enemyList.Count < 10)
        {
            GameObject spawned = Instantiate(enemy, new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 4.5f), Random.Range(-10f, 10f)), Quaternion.identity);
        }
        StartCoroutine(Spawner(timer, enemy));
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
