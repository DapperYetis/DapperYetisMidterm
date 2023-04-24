using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HybridEnemy : EnemyAI
{
    [Header("--- Components ---")]
    [SerializeField]
    protected List<Transform> _meleePos;
    [SerializeField]
    protected List<Transform> _shootPos;

    [Header("--- Melee Stats ---")]
    [SerializeField]
    protected float _attackDamage;
    [SerializeField]
    protected float _attackRate;
    [SerializeField]
    protected float _attackRange;
    [SerializeField]
    protected GameObject _clawOrFang;
    protected bool _inAttackRange;

    [Header("--- Ranged Stats ---")]
    [SerializeField]
    protected float _shootDamage;
    [SerializeField]
    protected float _shootRate;
    [SerializeField]
    protected int _shootDist;
    [SerializeField]
    protected float _shootSpread;
    [SerializeField]
    protected GameObject _bullet;
    [SerializeField]
    protected float _bulletSpeed;

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

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Attack);
        }
    }

    protected virtual void Attack()
    {
        if (_inAttackRange)
            Melee();
        else
            Shoot();
    }

    protected virtual void Melee()
    {
        if (this == null || !isActiveAndEnabled) return;

        StartCoroutine(TakeSwing());
    }

    protected virtual IEnumerator TakeSwing()
    {
        _anim.SetTrigger("Attack");

        Quaternion rot = Quaternion.LookRotation(_playerDirProjected * 0.5f);
        for (int i = 0; i < _meleePos.Count; i++)
        {
            Instantiate(_clawOrFang, _meleePos[i].position, rot).GetComponent<Rigidbody>().velocity = transform.forward;
        }

        yield return new WaitForSeconds(_attackRate);
        _isAttacking = false;
    }

    protected virtual void Shoot()
    {
        if (this == null || !isActiveAndEnabled) return;

        StartCoroutine(FireShot());
    }

    protected virtual IEnumerator FireShot()
    {
        _anim.SetTrigger("Shoot");
        Quaternion rot = Quaternion.LookRotation(_playerDirProjected * 0.5f);
        if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(_playerDir))) >= 60)
            rot = Quaternion.LookRotation(_playerDir);
        rot = Quaternion.RotateTowards(rot, Random.rotation, _shootSpread * GameManager.instance.player.movement.speedRatio);
        for (int i = 0; i < _shootPos.Count; i++)
        {
            Instantiate(_bullet, _shootPos[i].position, rot).GetComponent<Rigidbody>().velocity = transform.forward * _bulletSpeed;
        }

        yield return new WaitForSeconds(_shootRate);
        _isAttacking = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inAttackRange = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _inAttackRange = false;
        }
    }
}
