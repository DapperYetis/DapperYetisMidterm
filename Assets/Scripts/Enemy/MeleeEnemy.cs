using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI
{
    private List<IDamageable> _previouslyHit = new();
    protected bool _inAttackRange;

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking && _inAttackRange)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Melee, () => Mathf.FloorToInt(_playerDir.magnitude));
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

        for (int i = 0; i < _primaryAttackStats.positions.Length; i++)
        {
            AttackStart();
        }

        yield return new WaitForSeconds(_primaryAttackStats.rate);
        _isAttacking = false;
    }

    protected void AttackStart()
    {
        biteCol.enabled = true;
    }

    protected void AttackEnd()
    {
        biteCol.enabled = false;
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
