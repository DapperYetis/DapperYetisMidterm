using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    List<EnemyAI> _enemies;
    private Dictionary<SOWave, List<EnemyAI>> _enemiesByWave;
    [SerializeField]
    private List<SOWave> _waves;
    private List<Transform> _wavePoints;

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
        OnEnemyCountChange.AddListener(GameManager.instance.EndConditions);
        SetWaves();
    }

    public void SetWaves()
    {
        _enemies = new();
        _enemiesByWave = new();

        _wavePoints = (from go in GameObject.FindGameObjectsWithTag("EnemySpawnPoint") select go.transform).ToList();
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for(int i = 0; i < _waves.Count; ++i)
        {
            yield return new WaitForSeconds(RunWave(i));
        }
    }

    private float RunWave(int index)
    {
        _enemiesByWave[_waves[index]] = new();
        StartCoroutine(Spawner(_waves[index], GetSpawnPoint(), 0));

        return _waves[index].maxEnemyCount * _waves[index].spawnInterval;
    }

    private Transform GetSpawnPoint()
    {
        if (GameManager.instance.player == null) return _wavePoints[0];

        List<Transform> sortedPoints = (_wavePoints.OrderBy((trans) => (GameManager.instance.player.transform.position - trans.position).sqrMagnitude)).ToList();
        return sortedPoints[Random.Range(0, (int)(_wavePoints.Count * 0.1f))];
    }

    private IEnumerator Spawner(SOWave wave, Transform spawnPoint, int spawnedCount)
    {
        if (spawnedCount++ < wave.maxEnemyCount && spawnPoint != null)
        {
            EnemyAI enemy = Instantiate(wave.spawnPointPrefab, spawnPoint.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), Quaternion.identity).GetComponent<EnemyAI>();
            enemy.SetUp(wave);
            yield return new WaitForSeconds(wave.spawnInterval);
            StartCoroutine(Spawner(wave, spawnPoint, spawnedCount));
        }
    }

    public void AddEnemyToList(EnemyAI enemy, SOWave spawnPoint)
    {
        _enemies.Add(enemy);
        if (_enemiesByWave.ContainsKey(spawnPoint))
            _enemiesByWave[spawnPoint].Add(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy, SOWave spawnPoint)
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
        SetWaves();
    }
}
