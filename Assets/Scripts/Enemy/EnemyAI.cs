using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

public abstract class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("--- Components ---")]
    [SerializeField]
    protected Renderer _model;
    [SerializeField]
    protected NavMeshAgent _agent;
    [SerializeField]
    protected Animator _anim;

    [Header("--- NavMesh Mods ---")]
    [SerializeField]
    protected float _changeTime;
    [SerializeField]
    protected AnimationCurve _changeCurve;
    [SerializeField]
    protected float _radiusMod;
    [SerializeField]
    protected float _speedMod;

    // Events
    [HideInInspector]
    public UnityEvent OnHealthChange;

    // Enemy Stats
    [SerializeField]
    protected EnemyStats _stats;
    public EnemyStats stats => _stats;
    [SerializeField]
    protected EnemyStats _statsScaling;
    protected float _HPCurrent;
    protected float _angleToPlayer;
    protected float _stoppingDistOG;
    protected SOWave _spawnPoint;
    protected bool _isSetUp;
    protected bool _isAttacking;
    protected Vector3 _playerDir => GameManager.instance.player.transform.position - transform.position;
    protected bool _indicatingHit;

    [Space(20), SerializeField]
    protected EnemyAttackStats _primaryAttackStats;
    public EnemyAttackStats primaryAttackStats => _primaryAttackStats;
    [SerializeField]
    protected EnemyAttackStats _primaryAttackStatsScaling;

    protected Vector3 _playerDirProjected
    {
        get
        {
            float a = Vector3.Dot(GameManager.instance.player.movement.playerVelocity, GameManager.instance.player.movement.playerVelocity) - (_primaryAttackStats.speed * _primaryAttackStats.speed);
            float b = 2 * Vector3.Dot(GameManager.instance.player.movement.playerVelocity, _playerDir);
            float c = Vector3.Dot(_playerDir, _playerDir);

            float p = -b / (2 * a);
            float q = Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

            float time1 = p - q;
            float time2 = p + q;
            float timeActual = time1 > time2 && time2 > 0 ? time2 : time1;

            return _playerDir + GameManager.instance.player.movement.playerVelocity * timeActual;
        }
    }

    protected virtual void Start()
    {
        StartCoroutine(SizeChange());
    }

    protected virtual void Update()
    {
        if (!_isSetUp) return;

        if (_agent.isActiveAndEnabled)
            _agent.SetDestination(GameManager.instance.player.transform.position);
        
        FacePlayer();
    }

    // Creates the enemy avoidance system
    protected virtual IEnumerator SizeChange()
    {
        float startingRadius = _agent.radius;
        float startingSpeed = _agent.speed;
        float radius = Random.Range(_agent.radius, _agent.radius + _radiusMod);
        float speed = Random.Range(_agent.speed, _agent.speed + _speedMod);
        float startTime = Time.time;

        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        while (Time.time < startTime + _changeTime)
        {
            _agent.radius = Mathf.Lerp(startingRadius, radius, _changeCurve.Evaluate((Time.time - startTime) / _changeTime));
            _agent.speed = Mathf.Lerp(startingSpeed, speed, _changeCurve.Evaluate((Time.time - startTime) / _changeTime));
            yield return wait;
        }
        _agent.radius = radius;
        _agent.speed = speed;
    }

    public virtual void SetUp(SOWave spawnPoint)
    {
        _HPCurrent = _stats.HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
        _spawnPoint = spawnPoint;

        EnemyManager.instance.AddEnemyToList(this);
        _isSetUp = true;
    }

    public virtual void ScaleEnemy()
    {
        for(int i = 0; i < EnemyManager.instance.scaleFactorInt; ++i)
        {
            _stats = _stats + _statsScaling;
            _primaryAttackStats = _primaryAttackStats + _primaryAttackStatsScaling;
        }
    }

    protected virtual IEnumerator FlashColor(Color clr)
    {
        if(!_indicatingHit)
        {
            _indicatingHit = true;
            Color mainColor = _model.material.color;
            _model.material.color = clr;

            yield return new WaitForSeconds(0.1f);

            _model.material.color = mainColor;
            _indicatingHit = false;
        }
    }

    protected virtual void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(_playerDir.x, 0, _playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _stats.facePlayerSpeed);
    }

    public virtual void Damage(float amount)
    {
        _HPCurrent -= amount;

        if (_HPCurrent <= 0)
        {
            _anim.SetBool("Dead", true);
            GetComponent<CapsuleCollider>().enabled = false;
            _agent.enabled = false;
            enabled = false;
            EnemyManager.instance.RemoveEnemyFromList(this);
            Destroy(gameObject, 3);
        }
        else
        {
            _anim.SetTrigger("Damage");
            StartCoroutine(FlashColor(Color.red));
        }

        OnHealthChange.Invoke();
    }

    public virtual void Heal(float health)
    {
        _HPCurrent += health;
        StartCoroutine(FlashColor(Color.green));

        if (_HPCurrent >= _stats.HPMax)
            _HPCurrent = _stats.HPMax;

        OnHealthChange.Invoke();
    }

    public virtual float GetHealthMax()
    {
        return _stats.HPMax;
    }

    public virtual float GetHealthCurrent()
    {
        return _HPCurrent;
    }
}
