using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    List<EnemyAI> _enemies;
    private Dictionary<WaveStats, List<EnemyAI>> _enemiesByWave;
    private LevelWaveInfo _wavesInfo;

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

    public void SetWaves(LevelWaveInfo levelWaveInfo)
    {
        _wavesInfo = levelWaveInfo;
        _enemies = new();
        _enemiesByWave = new();

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (int i = 0; i < _wavesInfo.waveCount; i++)
        {
            Debug.Log("Wave " + (i + 1));
            int index = Random.Range(0, _wavesInfo.waves.Count);
            _enemiesByWave[_wavesInfo.waves[index]] = new();
            StartCoroutine(Spawner(_wavesInfo.waves[index], GetSpawnPoint(), 0));
            yield return new WaitForSeconds(_wavesInfo.waves[index].maxEnemyCount * _wavesInfo.waves[index].spawnInterval);
        }
    }

    private Transform GetSpawnPoint()
    {
        return _wavesInfo.spawnPoints[Random.Range(0, _wavesInfo.spawnPoints.Count)];
    }

    private IEnumerator Spawner(WaveStats wave, Transform spawnPoint, int spawnedCount)
    {
        yield return new WaitForSeconds(wave.spawnInterval);
        if (spawnedCount++ < wave.maxEnemyCount)
        {
            EnemyAI enemy = Instantiate(wave.spawnPointPrefab, spawnPoint.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), Quaternion.identity).GetComponent<EnemyAI>();
            enemy.SetUp(wave);
            StartCoroutine(Spawner(wave, spawnPoint, spawnedCount));
        }
    }

    public void AddEnemyToList(EnemyAI enemy, WaveStats spawnPoint)
    {
        _enemies.Add(enemy);
        if (_enemiesByWave.ContainsKey(spawnPoint))
            _enemiesByWave[spawnPoint].Add(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy, WaveStats spawnPoint)
    {
        _enemies.Remove(enemy);
        if(_enemiesByWave.ContainsKey(spawnPoint))
            _enemiesByWave[spawnPoint].Remove(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public int GetEnemyListSize()
    {
        return _enemies.Count;
    }

    public void ResetMap()
    {
        OnEnemyCountChange.RemoveAllListeners();
        foreach (EnemyAI enemy in _enemies)
        {
            Destroy(enemy.gameObject);
        }
        _enemies.Clear();
        _enemiesByWave.Clear();
    }
}
