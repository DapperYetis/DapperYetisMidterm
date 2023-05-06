using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

[RequireComponent(typeof(EnemyDrops))]
public abstract class EnemyAI : MonoBehaviour, IDamageable, IBuffable
{
    [Header("--- Components ---")]
    [SerializeField]
    protected Renderer _model;
    [SerializeField]
    protected NavMeshAgent _agent;
    [SerializeField]
    protected Animator _anim;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    protected EnemyDrops _drops;

    [Header("--- NavMesh Mods ---")]
    [SerializeField]
    protected float _changeTime;
    [SerializeField]
    protected AnimationCurve _changeCurve;
    [SerializeField]
    protected float _radiusMod;
    [SerializeField]
    protected float _speedMod;
    [SerializeField]
    protected float _animTransSpeed;

    // Events
    [HideInInspector]
    public UnityEvent _OnHealthChange;

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
    protected bool _hasCompletedAttack;
    protected Vector3 _playerDir => GameManager.instance.player.transform.position - transform.position + 2 * Vector3.down;
    protected bool _indicatingHit;
    protected float _speed;
    public Dictionary<SOBuff, (int stacks, float time)> _currentBuffs = new();
    protected int _moveType;
    [SerializeField]
    protected int _moveChances = 10;
    [SerializeField]
    protected float _runTypeTimer;
    [SerializeField]
    protected float _circlingRange = 10f;
    protected bool _hasEnteredRange => _playerDir.magnitude < _circlingRange;
    protected bool _movementOverride;

    [Header("--- Death Controls ---")]
    [SerializeField]
    protected float _deathDelay;
    [SerializeField]
    protected float _timeLength;
    [SerializeField]
    protected float _scaleMultiplier;

    [Space(20), SerializeField]
    protected EnemyAttackStats _primaryAttackStats;
    public EnemyAttackStats primaryAttackStats => _primaryAttackStats;
    [SerializeField]
    protected EnemyAttackStats _primaryAttackStatsScaling;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audSpawnVol;
    [SerializeField]
    protected AudioClip[] _audSpawn;
    [Range(0, 1)]
    [SerializeField]
    protected float _audSpawnRoarVol;
    [SerializeField]
    protected AudioClip[] _audSpawnRoar;
    [Range(0, 1)]
    [SerializeField]
    protected float _audGettingCloseVol;
    [SerializeField]
    protected AudioClip[] _audGettingClose;
    [Range(0, 1)]
    [SerializeField]
    protected float _audTakeDamageVol;
    [SerializeField]
    protected AudioClip[] _audTakeDamage;
    [Range(0, 1)]
    [SerializeField]
    protected float _audHealVol;
    [SerializeField]
    protected AudioClip[] _audHeal;
    [Range(0, 1)]
    [SerializeField]
    protected float _audBuffVol;
    [SerializeField]
    protected AudioClip[] _audBuff;
    [Range(0, 1)]
    [SerializeField]
    protected float _audDebuffVol;
    [SerializeField]
    protected AudioClip[] _audDebuff;
    [Range(0, 1)]
    [SerializeField]
    protected float _audDeathVol;
    [SerializeField]
    protected AudioClip[] _audDeath;
    [Range(0, 1)]
    [SerializeField]
    protected float _audFallDownVol;
    [SerializeField]
    protected AudioClip[] _audFallDown;

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
        _drops = GetComponent<EnemyDrops>();
        StartCoroutine(PickMoveType());
    }

    protected virtual void Update()
    {
        if (!_isSetUp) return;

        _anim.SetFloat("Speed", _speed);
        _speed = Mathf.Lerp(_speed, _agent.velocity.normalized.magnitude, Time.deltaTime * _animTransSpeed);

        if (_agent.isActiveAndEnabled)
        {
            Movement();
        }

        CheckBuffs();
    }

    protected IEnumerator PickMoveType()
    {
        _moveType = Random.Range(0, _moveChances);
        yield return new WaitForSeconds(_runTypeTimer);
        if (this != null)
            StartCoroutine(PickMoveType());
    }

    protected virtual void Movement()
    {
        if (_movementOverride) return;
        float distanceToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);

        if (_hasCompletedAttack) // If the enemy has already attacked, back off to let another enemy queue
        {
            _agent.SetDestination(transform.position - (_playerDir.normalized * _stats.speed));
            if (!_hasEnteredRange)
            {
                _hasCompletedAttack = false;
            }
        }
        else if (_hasEnteredRange)
        {
            if (_moveType % 2 == 0)
            {
                _agent.SetDestination(transform.position + (Vector3.Cross(_playerDir.normalized, Vector3.up) * _stats.speed));
            }
            else
            {
                _agent.SetDestination(transform.position + (Vector3.Cross(_playerDir.normalized, Vector3.up) * _stats.speed * -1));
            }
        }
        else
        {
            if (_moveType < _moveChances * 0.6f)
            {
                _agent.SetDestination(GameManager.instance.player.transform.position);
            }
            else if (_moveType % 2 == 0)
            {
                _agent.SetDestination(transform.position + (Vector3.Cross(_playerDir.normalized, Vector3.up) * _stats.speed));

            }
            else
            {
                _agent.SetDestination(transform.position + (Vector3.Cross(_playerDir.normalized, Vector3.up) * _stats.speed * -1));
            }
        }
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
        ScaleEnemy();
        _HPCurrent = _stats.HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
        _spawnPoint = spawnPoint;

        EnemyManager.instance.AddEnemyToList(this);
        _isSetUp = true;

        _agent.speed = _stats.speed;
        _agent.acceleration = _stats.acceleration;
    }

    public virtual void ScaleEnemy()
    {
        for (int i = 0; i < EnemyManager.instance.scaleFactorInt - 1; ++i)
        {
            _stats += _statsScaling;
            _primaryAttackStats += _primaryAttackStatsScaling;
        }
    }

    protected virtual IEnumerator FlashColor(Color clr)
    {
        if (!_indicatingHit)
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

    public virtual void Damage(float amount, (SOBuff buff, int amount)[] buffs)
    {
        _HPCurrent -= amount;

        if (_HPCurrent <= 0)
        {
            Die();
        }
        else
        {
            _anim.SetTrigger("Damage");
            _aud.PlayOneShot(_audTakeDamage[Random.Range(0, _audTakeDamage.Length)], _audTakeDamageVol);
            StartCoroutine(FlashColor(Color.red));
            if (buffs != null)
            {
                foreach (var buff in buffs)
                {
                    AddBuff(buff.buff, buff.amount);
                }
            }
        }

        _OnHealthChange.Invoke();
    }

    private void Die()
    {
        _aud.PlayOneShot(_audDeath[Random.Range(0, _audDeath.Length)], _audDeathVol);
        _anim.SetBool("Dead", true);
        GetComponent<CapsuleCollider>().enabled = false;
        _agent.enabled = false;
        enabled = false;
        EnemyManager.instance.RemoveEnemyFromList(this);
        _drops.Drop();
        _aud.PlayOneShot(_audFallDown[Random.Range(0, _audFallDown.Length)], _audFallDownVol);
        StartCoroutine(EnemyRemoved());
    }

    public virtual IEnumerator EnemyRemoved()
    {
        yield return new WaitForSeconds(_deathDelay);

        Vector3 scaleSize = transform.localScale;
        float startTime = Time.time;
        while (Time.time <= startTime + _timeLength)
        {
            yield return new WaitForEndOfFrame();
            // Enemy sinks into the ground
            transform.Translate(0, -Time.deltaTime * _model.bounds.size.y, 0, Space.World);
            // Enemy shrinks
            transform.localScale = scaleSize * (1 - Mathf.Clamp(_scaleMultiplier * (Time.time - startTime) / _timeLength, 0, 1));
        }

        Destroy(gameObject);
    }

    public virtual void Heal(float health)
    {
        _HPCurrent += health;

        if (_HPCurrent >= _stats.HPMax)
            _HPCurrent = _stats.HPMax;
        else
        {
            StartCoroutine(FlashColor(Color.green));
            _aud.PlayOneShot(_audHeal[Random.Range(0, _audHeal.Length)], _audHealVol);
        }

        _OnHealthChange.Invoke();
    }

    public virtual float GetHealthMax()
    {
        return _stats.HPMax;
    }

    public virtual float GetHealthCurrent()
    {
        return _HPCurrent;
    }

    public List<SOBuff> GetBuffs() => _currentBuffs.Keys.ToList();

    public void AddBuff(SOBuff buff, int amount = 1)
    {
        if (!_currentBuffs.ContainsKey(buff))
        {
            if (buff)
                _currentBuffs.Add(buff, (0, Time.time + buff.buffLength));
        }
        _currentBuffs[buff] = (_currentBuffs[buff].stacks + amount, _currentBuffs[buff].time);
    }

    public void AddBuffs(List<(SOBuff buff, int count)> buffCounts)
    {
        foreach (var buff in buffCounts)
            AddBuff(buff.buff, buff.count);
    }

    public int GetStackCount(SOBuff buff)
    {
        return _currentBuffs.ContainsKey(buff) ? _currentBuffs[buff].stacks : 0;
    }

    public bool RemoveBuff(SOBuff buff)
    {
        if (!_currentBuffs.ContainsKey(buff)) return false;

        switch (buff.removeType)
        {
            case BuffRemoveType.Single:
                _currentBuffs[buff] = (_currentBuffs[buff].stacks - 1, Time.time + buff.buffLength);
                break;
            case BuffRemoveType.Stack:
                _currentBuffs[buff] = (0, 0);
                break;
        }

        if (_currentBuffs[buff].stacks <= 0)
        {
            _currentBuffs.Remove(buff);
            return true;
        }

        return false;
    }

    private void CheckBuffs()
    {
        for (int i = 0; i < _currentBuffs.Count; ++i)
        {
            if (Time.time < _currentBuffs[_currentBuffs.Keys.ElementAt(i)].time) continue;

            if (RemoveBuff(_currentBuffs.Keys.ElementAt(i)))
                --i;
        }
    }
}
