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
    [Range(1, 100)][SerializeField] int _HPMax;
    [SerializeField] int _facePlayerSpeed;
    [SerializeField] int _visionAngle;
    int _HPCurrent;

    [Header("--- Gun Stats ---")]
    [Range(1, 10)][SerializeField] int _shotDamage;
    [Range(0.1f, 5)][SerializeField] float _fireRate;
    [Range(1, 100)][SerializeField] int _shootDist;
    [SerializeField] GameObject _bullet;
    [SerializeField] int _bulletSpeed;

    Vector3 _playerDir;
    bool _isPlayerInRange;
    float _angleToPlayer;
    bool _isShooting;
    float _stoppingDistOG;

    void Start()
    {
        _HPCurrent = _HPMax;
        _stoppingDistOG = _agent.stoppingDistance;
    }

    void Update()
    {
        if (_isPlayerInRange)
        {
            CanSeePlayer();
        }
    }

    bool CanSeePlayer()
    {
        _playerDir = (GameManager.instance.player.transform.position - _headPos.position);
        _angleToPlayer = Vector3.Angle(new Vector3(_playerDir.x, 0, _playerDir.z), transform.forward);

        Debug.DrawRay(_headPos.position, _playerDir, Color.yellow);
        Debug.Log(_angleToPlayer);

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

    IEnumerator FireShot()
    {
        _isShooting = true;

        GameObject bulletClone = Instantiate(_bullet, _shootPos.position, _bullet.transform.rotation);
        bulletClone.GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;

        yield return new WaitForSeconds(_fireRate);
        _isShooting = false;
    }

    IEnumerator flashColor(Color clr)
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

    public void Damage(int amount)
    {
        _HPCurrent -= amount;        
        StartCoroutine(flashColor(Color.red));

        if (_HPCurrent <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int health)
    {
        _HPCurrent += health;
        StartCoroutine(flashColor(Color.green));

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
}
