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
    // Waves management
    [SerializeField]
    private List<SOWave> _waves;
    private List<Transform> _wavePoints;

    // Combat management
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private int _attackBudgetMax;
    private int _attackBudget;
    private bool _isRunning;
    private Queue<System.Action> _attacks;

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

        _attackBudget = _attackBudgetMax;
        _attacks = new();

        SetWaves();
    }

    public void SetWaves()
    {
        _enemies = new();

        _wavePoints = (from go in GameObject.FindGameObjectsWithTag("EnemySpawnPoint") select go.transform).ToList();
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(5);
        for(int i = 0; i < _waves.Count; ++i)
        {
            yield return new WaitForSeconds(RunWave(i));
        }
    }

    private float RunWave(int index)
    {
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
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy, SOWave spawnPoint)
    {
        _enemies.Remove(enemy);
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
        SetWaves();
    }

    public void QueueAttack(System.Action attack)
    {
        _attacks.Enqueue(attack);
        if (!_isRunning)
            StartCoroutine(RunDequeue());
    }

    private IEnumerator RunDequeue()
    {
        _isRunning = true;
        if (_attacks.Count > 0 && _attackBudget > 0)
        {
            --_attackBudget;
            (_attacks.Dequeue()).Invoke();

            StartCoroutine(RunDequeue());
            yield return new WaitForSeconds(_attackTime);
            ++_attackBudget;
        }
        else
        {
            _isRunning = false;
            yield return null;
        }
    }
}
