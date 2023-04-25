using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI
{
    protected bool _inAttackRange;

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking && _inAttackRange)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Melee);
        }
    }

    protected virtual void Melee()
    {
        if (this == null || !isActiveAndEnabled) return;

        StartCoroutine(TakeSwing());
    }

    protected virtual IEnumerator TakeSwing()
    {
        _anim.SetTrigger("Attack");

        Quaternion rot = Quaternion.LookRotation(_playerDir * 0.5f);
        for (int i = 0; i < _primaryAttackStats.positions.Length; i++)
        {
            Instantiate(_primaryAttackStats.prefab, _primaryAttackStats.positions[i].position, rot).GetComponent<Projectile>().SetStats(_primaryAttackStats);
        }

        yield return new WaitForSeconds(_primaryAttackStats.rate);
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
