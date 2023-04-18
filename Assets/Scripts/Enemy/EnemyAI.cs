using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("--- Components ---")]
    [SerializeField] 
    protected Renderer _model;
    [SerializeField]
    protected NavMeshAgent _agent;
    [SerializeField]
    protected Transform _headPos;
    [SerializeField]
    protected Transform _shootPos;

    [Header("--- Enemy Stats ---")]
    [Range(1, 100)][SerializeField]
    protected float _HPMax;
    [SerializeField]
    protected int _facePlayerSpeed;
    [SerializeField]
    protected int _visionAngle;
    protected float _HPCurrent;
    [SerializeField]
    protected bool _isWandering;

    [Header("--- Gun Stats ---")]
    [Range(1, 10)][SerializeField]
    protected float _shotDamage;
    [Range(0.1f, 5)][SerializeField]
    protected float _fireRate;
    [Range(1, 100)][SerializeField]
    protected int _shootDist;
    [SerializeField]
    protected float _shootSpread;
    [SerializeField]
    protected GameObject _bullet;
    [SerializeField]
    protected float _bulletSpeed;

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

    protected Vector3 _playerDir => GameManager.instance.player.transform.position - _headPos.position;
    protected Vector3 _playerDirProjected
    {
        get
        {
            float a = Vector3.Dot(GameManager.instance.player.movement.playerVelocity, GameManager.instance.player.movement.playerVelocity) - (_bulletSpeed * _bulletSpeed);
            float b = 2 * Vector3.Dot(GameManager.instance.player.movement.playerVelocity, _playerDir);
            float c = Vector3.Dot(_playerDir, _playerDir);

            float p = -b / (2 * a);
            float q = Mathf.Sqrt((b * b) - 4 * a * c) / (2 * a);

            float time1 = p - q;
            float time2 = p + q;
            float timeActual;

            timeActual = time1 > time2 && time2 > 0 ? time2 : time1;

            return _playerDir + GameManager.instance.player.movement.playerVelocity * timeActual;
        }
    }
    protected bool _isPlayerInRange;
    protected float _angleToPlayer;
    protected bool _isShooting;
    protected float _stoppingDistOG;
    protected SOWave _spawnPoint;
    protected bool _isSetUp;

    protected virtual void Start()
    {
        StartCoroutine(SizeChange());
    }

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

        _agent.SetDestination(GameManager.instance.player.transform.position);

        if (_isPlayerInRange)
        {
            CanSeePlayer();
        }
    }

    public virtual void SetUp(SOWave spawnPoint)
    {
        _HPCurrent = _HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
        _spawnPoint = spawnPoint;

        EnemyManager.instance.AddEnemyToList(this);
        _isSetUp = true;
    }

    protected virtual bool CanSeePlayer()
    {
        _angleToPlayer = Vector3.Angle(new Vector3(_playerDir.x, 0, _playerDir.z), transform.forward);

        Debug.DrawRay(_headPos.position, _playerDir, Color.yellow);

        RaycastHit hit;
        if (Physics.Raycast(_headPos.position, _playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && _angleToPlayer <= _visionAngle)
            {
                _agent.stoppingDistance = _stoppingDistOG;


                if (_agent.remainingDistance < _agent.stoppingDistance)
                    FacePlayer();

                if (!_isShooting)
                {
                    _isShooting = true;
                    EnemyManager.instance.QueueAttack(Shoot);
                }

                return true;
            }
        }

        return false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }

    protected virtual void Shoot()
    {
        if (this == null) return;

        StartCoroutine(FireShot());
    }

    protected virtual IEnumerator FireShot()
    {
        Quaternion rot = Quaternion.LookRotation(_playerDirProjected * 0.5f);
        if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(_playerDir))) >= 60)
            rot = Quaternion.LookRotation(_playerDir);
        rot = Quaternion.RotateTowards(rot, Random.rotation, _shootSpread * GameManager.instance.player.movement.speedRatio);
        Instantiate(_bullet, _shootPos.position, rot).GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;

        yield return new WaitForSeconds(_fireRate);
        _isShooting = false;
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
        StartCoroutine(FlashColor(Color.red));

        if (_HPCurrent <= 0)
        {
            Destroy(gameObject);
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
