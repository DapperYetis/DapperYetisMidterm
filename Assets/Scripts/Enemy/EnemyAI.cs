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

    [Header("--- Enemy Stats ---")]
    [Range(1, 100)]
    [SerializeField]
    protected float _HPMax;
    [SerializeField]
    protected int _facePlayerSpeed;
    protected float _HPCurrent;
    protected float _angleToPlayer;
    protected float _stoppingDistOG;
    protected SOWave _spawnPoint;
    protected bool _isSetUp;
    protected bool _isAttacking;

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

    protected Vector3 _playerDir => GameManager.instance.player.transform.position - transform.position;

    protected virtual void Start()
    {
        StartCoroutine(SizeChange());
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

    protected virtual void Update()
    {
        if (!_isSetUp) return;

        if (_agent.isActiveAndEnabled)
            _agent.SetDestination(GameManager.instance.player.transform.position);
        
        FacePlayer();
    }

    public virtual void SetUp(SOWave spawnPoint)
    {
        _HPCurrent = _HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
        _spawnPoint = spawnPoint;

        EnemyManager.instance.AddEnemyToList(this);
        _isSetUp = true;
    }

    protected virtual IEnumerator FlashColor(Color clr)
    {
        Color mainColor = _model.material.color;
        _model.material.color = clr;
        yield return new WaitForSeconds(0.1f);
        _model.material.color = mainColor;
    }

    protected virtual void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(_playerDir.x, 0, _playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _facePlayerSpeed);
    }

    public virtual void Damage(float amount)
    {
        _HPCurrent -= amount;

        if (_HPCurrent <= 0)
        {
            StopAllCoroutines();
            _anim.SetBool("Dead", true);
            GetComponent<CapsuleCollider>().enabled = false;
            _agent.enabled = false;
            enabled = false;
            Destroy(gameObject, Time.deltaTime + 3);
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

        if (_HPCurrent >= _HPMax)
            _HPCurrent = _HPMax;

        OnHealthChange.Invoke();
    }

    public virtual float GetHealthMax()
    {
        return _HPMax;
    }

    public virtual float GetHealthCurrent()
    {
        return _HPCurrent;
    }

    protected virtual void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemyFromList(this);
    }
}
