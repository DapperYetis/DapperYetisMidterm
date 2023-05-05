using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HybridEnemy : MeleeEnemy
{
    [SerializeField]
    protected EnemyAttackStats _secondaryAttackStats;
    public EnemyAttackStats secondaryAttackStats => _secondaryAttackStats;
    [SerializeField]
    protected EnemyAttackStats _secondaryAttackStatsScaling;
    public EnemyAttackStats secondaryAttackStatsScaling => _secondaryAttackStatsScaling;

    protected override void Update()
    {
        _movementOverride = false;
        if (_primaryAttackStats.range > _playerDir.magnitude)
        {
            _movementOverride = true;
            _agent.SetDestination(GameManager.instance.player.transform.position);
        }
        else if (!_isAttacking)
        {
            _isAttacking = true;
            EnemyManager.instance.QueueAttack(Shoot, () => Mathf.FloorToInt(_playerDir.magnitude - _secondaryAttackStats.range), this);
        }

        base.Update();
    }

    public override void ScaleEnemy()
    {
        base.ScaleEnemy();
        for (int i = 0; i < EnemyManager.instance.scaleFactorInt - 1; ++i)
        {
            _secondaryAttackStats = _secondaryAttackStats + _secondaryAttackStatsScaling;
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
        Quaternion rot = Quaternion.LookRotation(_playerDir * 0.5f);
        if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(_playerDir))) >= 60)
            rot = Quaternion.LookRotation(_playerDir);
        rot = Quaternion.RotateTowards(rot, Random.rotation, _secondaryAttackStats.variance * GameManager.instance.player.movement.speedRatio);
        for (int i = 0; i < _secondaryAttackStats.positions.Length; i++)
        {
            Instantiate(_secondaryAttackStats.prefab, _secondaryAttackStats.positions[i].position, rot).GetComponent<Projectile>().SetStats(_secondaryAttackStats);
        }

        _hasCompletedAttack = true;
        yield return new WaitForSeconds(_secondaryAttackStats.rate);
        _isAttacking = false;
    }
}
