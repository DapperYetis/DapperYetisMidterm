using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    [SerializeField]
    protected GameObject _spawnEffectOpening;
    [SerializeField]
    protected GameObject _spawnEffectIdle;
    [SerializeField]
    protected GameObject _spawnEffectClosing;
    [SerializeField]
    protected CapsuleCollider _bodyCollider;

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
    public UnityEvent<SOBuff> _onBuffAdded;
    public UnityEvent<SOBuff> _onBuffRemoved;
    public UnityEvent OnEnemyDamaged;
    public UnityEvent<float> OnEnemyDamagedNumber;
    public UnityEvent OnBossDied;

    [Header("--- Stats ---")]
    [SerializeField, Space(20)]
    protected float _additivePriorityMod;
    [SerializeField]
    protected float _spawnCost = 1;
    public float spawnCost => _spawnCost;
    [SerializeField]
    protected EnemyStats _stats;
    public EnemyStats stats => _stats;
    protected EnemyStats _baseStats;
    [SerializeField]
    protected EnemyStats _statsScaling;
    protected float _HPCurrent;
    protected float _angleToPlayer;
    protected float _stoppingDistOG;
    protected SOWave _spawnPoint;
    protected bool _isSetUp;
    protected bool _isAttacking;
    protected bool _hasCompletedAttack;
    protected Vector3 _playerDir => VectorToPlayer(_primaryAttackStats);
    protected bool _indicatingHit;
    protected float _speed;
    public Dictionary<SOBuff, (int stacks, float time)> _currentBuffs = new();
    public Dictionary<SOBuff, BuffEffect> _currentBuffEffects = new();
    protected int _moveType;
    [SerializeField]
    protected int _moveChances = 10;
    [SerializeField]
    protected float _runTypeTimer;
    [SerializeField]
    protected float _circlingRange = 10f;
    protected bool _hasEnteredRange;
    protected bool _movementOverride;
    float _PortalOpenDur;
    float _PortalIdleDur;
    float _PortalCloseDur;

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
    protected EnemyAttackStats _basePrimaryAttackStats;

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
        get => ProjectedVectorToPlayer(_primaryAttackStats);
    }

    protected Vector3 VectorToPlayer(EnemyAttackStats stats) => GameManager.instance.player.transform.position - (stats.positions.Length > 0 ? stats.positions[0].position : transform.position);

    protected Vector3 ProjectedVectorToPlayer(EnemyAttackStats stats)
    {
        if (GameManager.instance.player.movement.playerVelocity.sqrMagnitude < 0.5f)
            return VectorToPlayer(stats);

        float a = Vector3.Dot(GameManager.instance.player.movement.playerVelocity, GameManager.instance.player.movement.playerVelocity) - (stats.speed * stats.speed);
        float b = 2 * Vector3.Dot(GameManager.instance.player.movement.playerVelocity, VectorToPlayer(stats));
        float c = Vector3.Dot(VectorToPlayer(stats), VectorToPlayer(stats));

        float p = -b / (2 * a);
        float q = Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

        float time1 = p - q;
        float time2 = p + q;
        float timeActual = time1 > time2 && time2 > 0 ? time2 : time1;

        return VectorToPlayer(stats) + GameManager.instance.player.movement.playerVelocity * timeActual;
    }

    protected virtual void Start()
    {
        _PortalOpenDur = _spawnEffectOpening.GetComponent<ParticleSystem>().main.duration;
        _PortalIdleDur = _spawnEffectIdle.GetComponent<ParticleSystem>().main.duration;
        _PortalCloseDur = _spawnEffectClosing.GetComponent<ParticleSystem>().main.duration;
        StartCoroutine(SizeChange());
        _drops = GetComponent<EnemyDrops>();
        StartCoroutine(PickMoveType());
        FacePlayer();
    }

    protected virtual void Update()
    {
        if (!_isSetUp) return;

        _anim.SetFloat("Speed", _speed);
        _speed = Mathf.Lerp(_speed, _agent.velocity.normalized.magnitude, Time.deltaTime * _animTransSpeed);

        if (_agent.isActiveAndEnabled && _HPCurrent > 0)
        {
            Movement();
        }

        CheckBuffs();
    }

    protected virtual float AttackPriority()
    {
        return (GameManager.instance.player.transform.position - transform.position).sqrMagnitude - _primaryAttackStats.range - _additivePriorityMod;
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
        if (!_hasEnteredRange && _playerDir.magnitude < _circlingRange)
        {
            _hasEnteredRange = true;
            _aud.PlayOneShot(_audGettingClose[Random.Range(0, _audGettingClose.Length)], _audGettingCloseVol);
        }

        if (_movementOverride) return;
        float distanceToPlayer = Vector3.Distance(GameManager.instance.player.transform.position, transform.position);

        if (_hasCompletedAttack) // If the enemy has already attacked, back off to let another enemy queue
        {
            _agent.SetDestination(transform.position - (_playerDir.normalized * _stats.speed));
            if (_hasEnteredRange)
            {
                _hasCompletedAttack = false;
                _hasEnteredRange = false;
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

    protected virtual IEnumerator SpawnInEffect()
    {
        _agent.enabled = true;
        FacePlayer();
        float animSpeedOrig = _anim.speed;
        _anim.speed = 0;
        _agent.enabled = false;
        _bodyCollider.enabled = false;
        enabled = false;

        Color mainColor = _model.material.color;
        _model.material.color = new Color(0, 0, 0, 0f); // should but doesnt make enemies transparent.  You have to make the material transparent and that causes visual problems so that was scrapped, but this is still good to have for other reasons.
        if (_audSpawn.Length > 0)
            _aud.PlayOneShot(_audSpawn[Random.Range(0, _audSpawn.Length)], _audSpawnVol);
        else
            Debug.LogWarning("No Spawn Sounds to play!");

        GameObject spFX1 = Instantiate(_spawnEffectOpening, transform, worldPositionStays: false);
        yield return new WaitForSeconds(_PortalOpenDur);
        Destroy(spFX1);

        GameObject spFX2 = Instantiate(_spawnEffectIdle, transform, worldPositionStays: false);
        yield return new WaitForSeconds(_PortalIdleDur + 1);
        Destroy(spFX2);
        //float timer = 5;
        //float max = 6;
        //spFX2.transform.localScale = new Vector3(Mathf.Lerp(0.1f, max, timer), Mathf.Lerp(0.1f, max, timer), Mathf.Lerp(0.1f, max, timer));
        _model.material.color = mainColor;

        GameObject spFX3 = Instantiate(_spawnEffectClosing, transform, worldPositionStays: false);
        yield return new WaitForSeconds(_PortalCloseDur);
        Destroy(spFX3);

        _anim.speed = animSpeedOrig;
    }

    protected virtual void SpawnRoar()
    {
        if (_audSpawnRoar.Length > 0)
            _aud.PlayOneShot(_audSpawnRoar[Random.Range(0, _audSpawnRoar.Length)], _audSpawnRoarVol);
        else
            Debug.LogWarning("No Spawn Roar Sounds to play!");
        Debug.Log($"{name} played a sound");
    }

    protected virtual void ReadyToFight()
    {
        _anim.speed = 1;
        _agent.enabled = true;
        _bodyCollider.enabled = true;
        enabled = true;
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

        _baseStats = _stats;
        _basePrimaryAttackStats = _primaryAttackStats;
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
        OnEnemyDamaged.Invoke();
        OnEnemyDamagedNumber.Invoke(amount);

        if (_HPCurrent <= 0)
        {
            OnBossDied.Invoke();
            Die();
        }
        else
        {
            _anim.SetTrigger("Damage");
            if (_audTakeDamage.Length > 0)
                _aud.PlayOneShot(_audTakeDamage[Random.Range(0, _audTakeDamage.Length)], _audTakeDamageVol);
            else
                Debug.LogWarning("No Take Damage Sounds to play!");
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

    protected virtual void Die()
    {
        _aud.enabled = false;
        _aud.enabled = true;
        _anim.SetBool("Dead", true);
        _bodyCollider.enabled = false;
        _agent.speed = 0;
        enabled = false;
        _agent.SetDestination(transform.position);
        EnemyManager.instance.RemoveEnemyFromList(this);
        _drops.Drop();
        StartCoroutine(EnemyRemoved());
    }

    protected void DeathCry()
    {
        if (_audDeath.Length > 0)
            _aud.PlayOneShot(_audDeath[Random.Range(0, _audDeath.Length)], _audDeathVol);
        else
            Debug.LogWarning("No Death Sounds to play!");
    }

    protected void FellDownDead()
    {
        if (_audFallDown.Length > 0)
            _aud.PlayOneShot(_audFallDown[Random.Range(0, _audFallDown.Length)], _audFallDownVol);
        else
            Debug.LogWarning("No Fall Down Sounds to play!");
    }

    protected virtual IEnumerator EnemyRemoved()
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

    public virtual void Heal(float health, bool silent = false)
    {
        _HPCurrent += health;

        if (_HPCurrent >= _stats.HPMax)
            _HPCurrent = _stats.HPMax;
        else
        {
            StartCoroutine(FlashColor(Color.green));
            if (_audHeal.Length > 0)
                _aud.PlayOneShot(_audHeal[Random.Range(0, _audHeal.Length)], _audHealVol);
            else
                Debug.LogWarning("No Heal Sounds to play!");
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
            _onBuffAdded.Invoke(buff);
            _currentBuffs.Add(buff, (0, Time.time + buff.buffLength));
            _currentBuffEffects.Add(buff, Instantiate(buff.effectPrefab, transform).GetComponent<BuffEffect>());
            _currentBuffEffects[buff].SetUp(this, buff);
            BuffStats(buff);
            
            if (buff.audioClips.Length > 0)
                _aud.PlayOneShot(buff.audioClips[Random.Range(0, buff.audioClips.Length)], buff.audioVolume);
            else
                Debug.LogWarning("No Buff/Debuff Sounds to play!");
        }
        _currentBuffs[buff] = (_currentBuffs[buff].stacks + amount, _currentBuffs[buff].time);
    }

    protected virtual void BuffStats(SOBuff buff, bool invert = false)
    {
        if (buff.generalMods.changeType == StatChangeType.Additive)
        {
            _stats += (invert ? -1 : 1) * buff.generalMods;
        }
        else
        {
            _stats += (invert ? -1 : 1) * (_baseStats * buff.generalMods);
        }

        if (buff.abilityMods.changeType == StatChangeType.Additive)
        {
            _primaryAttackStats += (invert ? -1 : 1) * (EnemyAttackStats)buff.abilityMods;
        }
        else
        {
            _primaryAttackStats += (invert ? -1 : 1) * (_basePrimaryAttackStats * (EnemyAttackStats)buff.abilityMods);
        }
        _agent.speed = _stats.speed;
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
                _currentBuffEffects[buff].RemoveStacks(1);
                break;
            case BuffRemoveType.Stack:
                _currentBuffs[buff] = (0, 0);
                _currentBuffEffects[buff].RemoveStacks(_currentBuffEffects[buff].stacks);
                break;
        }

        BuffStats(buff, true);

        if (_currentBuffs[buff].stacks <= 0)
        {
            _onBuffRemoved.Invoke(buff);
            _currentBuffs.Remove(buff);
            _currentBuffEffects.Remove(buff);
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
