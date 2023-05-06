using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyAI
{
    protected override void Update()
    {
        base.Update();

        if (!_isAttacking)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Shoot, () => Mathf.FloorToInt(_playerDir.magnitude - _primaryAttackStats.range), this);
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
        _aud.PlayOneShot(_primaryAttackStats._attackAudio[Random.Range(0, _primaryAttackStats._attackAudio.Length)], _primaryAttackStats._attackAudioVol);
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
