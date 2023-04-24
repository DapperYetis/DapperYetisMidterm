using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemy : EnemyAI
{
    [Header("--- Components ---")]
    [SerializeField]
    protected List<Transform> _meleePos;

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

    protected override void Update()
    {
        base.Update();

        if (!_isAttacking)
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
        for (int i = 0; i < _meleePos.Count; i++)
        {
            Instantiate(_clawOrFang, _meleePos[i].position, rot).GetComponent<Rigidbody>().velocity = transform.forward;
        }

        yield return new WaitForSeconds(_attackRate);
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
