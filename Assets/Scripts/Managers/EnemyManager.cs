using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using UnityEngine.AI;

public class EnemyManager : MonoBehaviour
{
    private static EnemyManager _instance;
    public static EnemyManager instance => _instance;

    // Waves management
    private bool _inBossRoom;
    public bool inBossRoom => _inBossRoom;
    private bool _withinSpawningBudget => _spawningBudget >= 0;
    [Header("---Waves---")]
    [SerializeField]
    private int _spawningBudgetMax;
    private int _spawningBudget;
    public int spawningBudget => _spawningBudget;
    [SerializeField]
    private int _initialAdditionalWaves = 0;
    [SerializeField]
    private float _initialWaveWait = 1.5f;
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
    public float scaleFactor => Mathf.Log(_scaleRate * ((GameManager.instance.runTimeMinutes + _startingMinutes) + _scaleRateInverse)) + 1;
    public int scaleFactorInt => Mathf.FloorToInt(scaleFactor);
    [SerializeField]
    private float _startingMinutes = 10;
    private List<EnemyAI> _enemies;
    public List<EnemyAI> enemies => _enemies;
    [SerializeField]
    private List<SOWave> _waves;
    private List<EnemySpawnPoint> _wavePoints = new();

    // Combat management
    [Header("---Combat Management---")]
    [SerializeField]
    private float _attackTime;
    [SerializeField]
    private int _attackBudgetMax;
    private int _attackBudget;
    private bool _isRunning;
    private List<(System.Action func, System.Func<float> priority, EnemyAI enemy)> _attacks;

    [HideInInspector]
    public UnityEvent OnEnemyCountChange;
    [HideInInspector]
    public UnityEvent<SOWave> OnBossRoomEnter;
    [HideInInspector]
    public UnityEvent<int> OnBossRoomLeave;

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
        _spawningBudget = _spawningBudgetMax;
        _attacks = new();

        SetWaves();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var point in _wavePoints)
        {
            Gizmos.DrawSphere(point.transform.position, 1);
        }
    }

    public void SetWaves()
    {
        _enemies = new();

        _waves = (from wave in _waves orderby SOWave.CalcSpawnCost(wave.enemyType.GetComponent<EnemyAI>(), wave.maxEnemyCount) ascending select wave).ToList();
        _wavePoints = (from go in GameObject.FindGameObjectsWithTag("EnemySpawnPoint") select go.GetComponent<EnemySpawnPoint>()).ToList();
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        yield return new WaitForSeconds(_initialWaveWait);
        Debug.Log($"Time: {GameManager.instance.runTimeMinutes} min.\tSpawning: {_initialAdditionalWaves} ADDITIONAL waves.");
        for (int i = 0; i < _initialAdditionalWaves; ++i)
        {
            RunWave(Random.Range(0, _waves.Count));
        }
        float waitTime = 0;
        while (GameManager.instance.inGame)
        {
            for (int i = 0; i < scaleFactorInt; ++i)
            {
                waitTime = RunWave(Random.Range(0, _waves.Count));
            }
            Debug.Log($"Time: {GameManager.instance.runTimeMinutes} min.\tSpawning: {scaleFactorInt} waves.");
            yield return new WaitForSeconds(waitTime);
        }
    }

    private float RunWave(int index)
    {
        if (_withinSpawningBudget)
            StartCoroutine(Spawner(_waves[index], GetSpawnPoint(), 0));

        return _waves[index].maxEnemyCount * _waves[index].spawnInterval + _timeBetweenWaves.Evaluate(GameManager.instance.runTimeMinutes);
    }

    private Vector3 GetSpawnPoint()
    {
        if (GameManager.instance.player == null) return Vector3.zero;

        List<EnemySpawnPoint> sortedPoints = (from point in _wavePoints where (GameManager.instance.player.transform.position - point.transform.position).magnitude > _minSpawnDistanceFromPlayer orderby (GameManager.instance.player.transform.position - point.transform.position).sqrMagnitude ascending select point).ToList();
        return sortedPoints.Count > 0 ? sortedPoints[Mathf.FloorToInt(_waveDistance.Evaluate(Random.Range(0f, 1f)))].GetPointInRange() : Vector3.zero;
    }

    private IEnumerator Spawner(SOWave wave, Vector3 spawnPoint, int spawnedCount)
    {
        if (!_inBossRoom && spawnedCount++ < wave.maxEnemyCount && spawnPoint != null)
        {
            SpawnEnemy(wave, spawnPoint);
            yield return new WaitForSeconds(wave.spawnInterval);
            StartCoroutine(Spawner(wave, spawnPoint, spawnedCount));
        }
    }

    public static void SpawnEnemy(SOWave wave, Vector3 spawnPosition)
    {
        Vector3 PossibleSpawnPoint = spawnPosition + new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f));
        //NavMeshPathStatus a;
        //if (_enemy.agent.pathEndPosition == PossibleSpawnPoint)
        //{
        //    a = PossibleSpawnPoint;
        //}

        EnemyAI enemy = Instantiate(wave.enemyType, PossibleSpawnPoint, Quaternion.identity).GetComponent<EnemyAI>();
        enemy.SetUp(wave);
    }

    public void AddEnemyToList(EnemyAI enemy)
    {
        _enemies.Add(enemy);
        _spawningBudget -= enemy.spawnCost;
        OnEnemyCountChange?.Invoke();
    }

    public void RemoveEnemyFromList(EnemyAI enemy)
    {
        _enemies.Remove(enemy);
        _spawningBudget += enemy.spawnCost;
        OnEnemyCountChange?.Invoke();
    }

    public int GetEnemyListSize()
    {
        return _enemies.Count;
    }

    public void ResetMap()
    {
        StopAllCoroutines();
        OnEnemyCountChange.RemoveAllListeners();

        _enemies.Clear();
        SetWaves();
    }

    public void QueueAttack(System.Action attack, System.Func<float> getPriority, EnemyAI enemyAttacking)
    {
        _attacks.Add((attack, getPriority, enemyAttacking));
        _attacks = (from queuedAttack in _attacks orderby queuedAttack.priority.Invoke() ascending select queuedAttack).ToList();
        if (!_isRunning)
            StartCoroutine(RunDequeue());
    }

    public void RemoveBadAttacks()
    {
        for (int i = 0; i < _attacks.Count; i++)
        {
            if (_attacks[i].enemy == null)
            {
                _attacks.RemoveAt(i);
                --i;
            }
        }
    }

    private IEnumerator RunDequeue()
    {
        _isRunning = true;
        RemoveBadAttacks();
        if (_attacks.Count > 0 && _attackBudget > 0)
        {
            int index = _attacks.Count >= 3 ? Random.Range(0, 3) : 0;
            --_attackBudget;
            _attacks[index].func.Invoke();
            _attacks.RemoveAt(index);

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

    public void RequestDequeue(EnemyAI enemy)
    {
        for (int i = 0; i < _attacks.Count; i++)
        {
            if (_attacks[i].enemy == enemy)
            {
                RunDequeue();
            }
        }
    }

    public void EnterBossRoom(SOWave wave)
    {
        _inBossRoom = true;
        OnBossRoomEnter.Invoke(wave);
    }

    public void LeaveBossRoom(int nextLevel)
    {
        _inBossRoom = false;
        OnBossRoomLeave.Invoke(nextLevel);
    }
}
