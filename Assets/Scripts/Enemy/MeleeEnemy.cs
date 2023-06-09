using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
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

        if (!_isAttacking && _hasEnteredRange)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Melee, AttackPriority, this);
        }

        base.Update();
    }

    protected override void Movement()
    {
        if (_isAttacking)
        {
            FacePlayer();
            _agent.SetDestination(GameManager.instance.player.transform.position);
        }
        else
            base.Movement();
    }

    protected virtual void Melee()
    {
        if (this == null || !isActiveAndEnabled || _HPCurrent <= 0) return;

        StartCoroutine(TakeSwing());
    }

    protected virtual IEnumerator TakeSwing()
    {
        _anim.SetTrigger("Attack");
        if (_primaryAttackStats._attackAudio.Length > 0)
            _aud.PlayOneShot(_primaryAttackStats._attackAudio[Random.Range(0, _primaryAttackStats._attackAudio.Length)], _primaryAttackStats._attackAudioVol);
        else
            Debug.LogWarning("No Melee Attack Sounds to play!");
        _hasCompletedAttack = true;
        yield return new WaitForSeconds(_primaryAttackStats.rate);
        _isAttacking = false;
    }

    protected virtual void AttackStart()
    {
        _meleeCollider.enabled = true;
    }

    protected virtual void AttackEnd()
    {
        _meleeCollider.enabled = false;
    }
}
