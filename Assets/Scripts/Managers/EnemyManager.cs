using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    // Waves management
    [SerializeField]
    private AnimationCurve _timeBetweenWaves;
    public AnimationCurve timeBetweenWaves => _timeBetweenWaves;
    [SerializeField]
    private float _minSpawnDistanceFromPlayer;
    [SerializeField]
    private AnimationCurve _waveDistance;
    [SerializeField]
    private float _scaleRate;
    public float scaleRate => _scaleRate;
    private float _scaleRateInverse;
    public float scaleRateInverse => _scaleRateInverse;
    public float scaleFactor => Mathf.Log(_scaleRate * (GameManager.instance.runTimeMinutes + _scaleRateInverse)) + 1;
    public int scaleFactorInt => Mathf.FloorToInt(scaleFactor);
    private List<EnemyAI> _enemies;
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

        _scaleRateInverse = 1 / _scaleRate;
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
        float waitTime = 0;
        while(GameManager.instance.inGame)
        {
            for(int i = 0; i < scaleFactorInt; ++i)
            {
                waitTime = RunWave(Random.Range(0, _waves.Count));
            }
            Debug.Log($"Time: {GameManager.instance.runTimeMinutes} min.\tSpawning: {scaleFactorInt} waves.");
            yield return new WaitForSeconds(waitTime);
        }
    }

    private float RunWave(int index)
    {
        StartCoroutine(Spawner(_waves[index], GetSpawnPoint(), 0));

        return _waves[index].maxEnemyCount * _waves[index].spawnInterval + _timeBetweenWaves.Evaluate(GameManager.instance.runTimeMinutes);
    }

    private Transform GetSpawnPoint()
    {
        if (GameManager.instance.player == null) return _wavePoints[0];

        List<Transform> sortedPoints = _wavePoints.OrderBy((trans) => (GameManager.instance.player.transform.position - trans.position).sqrMagnitude).Where((trans) => (GameManager.instance.player.transform.position - trans.position).magnitude > _minSpawnDistanceFromPlayer).ToList();
        return sortedPoints[Mathf.FloorToInt(_waveDistance.Evaluate(Random.Range(0f,1f)))];
    }

    private IEnumerator Spawner(SOWave wave, Transform spawnPoint, int spawnedCount)
    {
        if (spawnedCount++ < wave.maxEnemyCount && spawnPoint != null)
        {
            EnemyAI enemy = Instantiate(wave.enemyType, spawnPoint.position + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), Quaternion.identity).GetComponent<EnemyAI>();
            enemy.SetUp(wave);
            yield return new WaitForSeconds(wave.spawnInterval);
            StartCoroutine(Spawner(wave, spawnPoint, spawnedCount));
        }
    }

    public void AddEnemyToList(EnemyAI enemy)
    {
        _enemies.Add(enemy);
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy)
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
