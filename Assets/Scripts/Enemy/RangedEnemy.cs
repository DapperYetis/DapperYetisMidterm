using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyAI
{
    protected override void Update()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Shoot, AttackPriority, this);
        }

        base.Update();
    }

    protected override void Movement()
    {
        FacePlayer();
        _agent.SetDestination(GameManager.instance.player.transform.position);
    }

    protected override float AttackPriority()
    {
        return base.AttackPriority() + _primaryAttackStats.range - GameManager.instance.player.transform.position.magnitude;
    }

    public override void ScaleEnemy()
    {
        base.ScaleEnemy();
        for (int i = 0; i < EnemyManager.instance.scaleFactorInt - 1; ++i)
        {
            _primaryAttackStats += _primaryAttackStatsScaling;
        }
    }

    protected virtual void Shoot()
    {
        if (this == null || !isActiveAndEnabled) return;

        StartCoroutine(FireShot());
    }

    protected virtual IEnumerator FireShot()
    {
        _anim.SetTrigger("Shoot");
        if (_primaryAttackStats._attackAudio.Length > 0)
        {
            if (_primaryAttackStats._attackAudio.Length > 0)
                _aud.PlayOneShot(_primaryAttackStats._attackAudio[Random.Range(0, _primaryAttackStats._attackAudio.Length)], _primaryAttackStats._attackAudioVol);
            else
                Debug.LogWarning("No Ranged Attack Sounds to play!");
        }

        Quaternion rot = Quaternion.LookRotation(_playerDirProjected * 0.5f);
        if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(_playerDir))) >= 60)
            rot = Quaternion.LookRotation(_playerDir);
        rot = Quaternion.RotateTowards(rot, Random.rotation, _primaryAttackStats.variance * GameManager.instance.player.movement.speedRatio);
        for (int i = 0; i < _primaryAttackStats.positions.Length; i++)
        {
            Instantiate(_primaryAttackStats.prefab, _primaryAttackStats.positions[i].position, rot).GetComponent<EnemyProjectile>().SetStats(_primaryAttackStats);
        }

        _hasCompletedAttack = true;
        yield return new WaitForSeconds(_primaryAttackStats.rate);
        _isAttacking = false;
    }
}
