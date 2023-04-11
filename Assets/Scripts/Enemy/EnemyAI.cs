using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("--- Components ---")]
    [SerializeField] Renderer _model;
    [SerializeField] NavMeshAgent _agent;
    [SerializeField] Transform _headPos;
    [SerializeField] Transform _shootPos;

    [Header("--- Enemy Stats ---")]
    [Range(1, 100)][SerializeField] float _HPMax;
    [SerializeField] int _facePlayerSpeed;
    [SerializeField] int _visionAngle;
    float _HPCurrent;

    [Header("--- Gun Stats ---")]
    [Range(1, 10)][SerializeField] float _shotDamage;
    [Range(0.1f, 5)][SerializeField] float _fireRate;
    [Range(1, 100)][SerializeField] int _shootDist;
    [SerializeField] GameObject _bullet;
    [SerializeField] float _bulletSpeed;

    Vector3 _playerDir;
    bool _isPlayerInRange;
    float _angleToPlayer;
    bool _isShooting;
    float _stoppingDistOG;
    EnemySpawnStats _spawnPoint;
    private bool _isSetUp;

    void Update()
    {
        if (!_isSetUp) return;

        if (_isPlayerInRange)
        {
            CanSeePlayer();
        }
    }

    public void SetUp(EnemySpawnStats spawnPoint)
    {
        _HPCurrent = _HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
        _spawnPoint = spawnPoint;

        EnemyManager.instance.AddEnemyToList(this, _spawnPoint);
        _isSetUp = true;
    }

    bool CanSeePlayer()
    {
        _playerDir = (GameManager.instance.player.transform.position - _headPos.position);
        _angleToPlayer = Vector3.Angle(new Vector3(_playerDir.x, 0, _playerDir.z), transform.forward);

        Debug.DrawRay(_headPos.position, _playerDir, Color.yellow);
        //Debug.Log(_angleToPlayer);

        RaycastHit hit;

        if (Physics.Raycast(_headPos.position, _playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && _angleToPlayer <= _visionAngle)
            {
                _agent.stoppingDistance = _stoppingDistOG;
                _agent.SetDestination(GameManager.instance.player.transform.position);

                if (_agent.remainingDistance < _agent.stoppingDistance)
                    FacePlayer();

                if (!_isShooting)
                    StartCoroutine(FireShot());

                return true;
            }
        }

        return false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInRange = false;
        }
    }

    void IsDisturbed()
    {
        // Means the enemy was shot from outside it's range, so it runs to the location of the origin of the shot
        if (!_isPlayerInRange)
        {
            _agent.SetDestination(GameManager.instance.player.transform.position);
            _agent.stoppingDistance = 0;

            //float dist = agent.remainingDistance; 
            //if (dist != Mathf.infinite && agent.pathStatus == NavMeshPathStatus.completed && agent.remainingDistance == 0) //Arrived.
            //{ }
            //
            //if (_agent.po)
            //{
            //
            //}
        }
    }

    void Wander()
    {
        if (!CanSeePlayer())
        {

        }
    }

    IEnumerator FireShot()
    {
        _isShooting = true;

        GameObject bulletClone = Instantiate(_bullet, _shootPos.position, Quaternion.LookRotation(_playerDir));
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;

        yield return new WaitForSeconds(_fireRate);
        _isShooting = false;
    }

    IEnumerator FlashColor(Color clr)
    {
        Color mainColor = _model.material.color;
        _model.material.color = clr;
        yield return new WaitForSeconds(0.1f);
        _model.material.color = mainColor;
    }

    void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(_playerDir.x, 0, _playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * _facePlayerSpeed);
    }

    public void Damage(float amount)
    {
        _HPCurrent -= amount;
        StartCoroutine(FlashColor(Color.red));

        if (_HPCurrent <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(float health)
    {
        _HPCurrent += health;
        StartCoroutine(FlashColor(Color.green));

        if (_HPCurrent >= _HPMax)
            _HPCurrent = _HPMax;
    }

    public float GetHealthMax()
    {
        return _HPMax;
    }

    public float GetHealthCurrent()
    {
        return _HPCurrent;
    }

    private void OnDestroy()
    {
        EnemyManager.instance.RemoveEnemyFromList(this, _spawnPoint);
    }
}
