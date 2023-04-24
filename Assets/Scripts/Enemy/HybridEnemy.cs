using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HybridEnemy : MeleeEnemy
{
    [SerializeField]
    protected EnemyAttackStats _secondaryAttackStats;
    public EnemyAttackStats secondaryAttackStats => _secondaryAttackStats;

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
        rot = Quaternion.RotateTowards(rot, Random.rotation, _secondaryAttackStats.variance * GameManager.instance.player.movement.speedRatio);
        for (int i = 0; i < _secondaryAttackStats.positions.Length; i++)
        {
            Instantiate(_secondaryAttackStats.prefab, _secondaryAttackStats.positions[i].position, rot).GetComponent<Rigidbody>().velocity = transform.forward * _secondaryAttackStats.speed;
        }

        yield return new WaitForSeconds(_secondaryAttackStats.rate);
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
