using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MeleeEnemy : EnemyAI
{
    private List<IDamageable> _previouslyHit = new();
    protected bool _inAttackRange => _playerDir.magnitude <= _primaryAttackStats.range;
    [Header("---Melee Components---")]
    [SerializeField, FormerlySerializedAs("_biteCol")]
    protected Collider _meleeCollider;

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


        yield return new WaitForSeconds(_primaryAttackStats.rate);
        _isAttacking = false;
    }

    protected void AttackStart()
    {
        _meleeCollider.enabled = true;
    }

    protected void AttackEnd()
    {
        _meleeCollider.enabled = false;
    }
}
