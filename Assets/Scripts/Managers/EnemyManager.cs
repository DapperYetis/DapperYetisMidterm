using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    [SerializeField] List<GameObject> _enemyList;

    [SerializeField] GameObject _enemyA;
    [SerializeField] float _timerA;

    void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _enemyList = new List<GameObject>();

        StartCoroutine(Spawner(_timerA, _enemyA));
    }

    void Update()
    {
        
    }

    private IEnumerator Spawner(float timer, GameObject enemy)
    {
        if (_enemyList.Count <= 10)
        {
            yield return new WaitForSeconds(timer);
            GameObject spawned = Instantiate(enemy, new Vector3(Random.Range(-10f, 10f), Random.Range(0f, 4.5f), Random.Range(-10f, 10f)), Quaternion.identity);
            StartCoroutine(Spawner(timer, enemy));
            AddEnemyToList(enemy);
        }
    }

    public void AddEnemyToList(GameObject enemy)
    {
        _enemyList.Add(enemy);
    }

    public void RemoveEnemyFromList(GameObject enemy)
    {
        _enemyList.Remove(enemy);
    }
}
