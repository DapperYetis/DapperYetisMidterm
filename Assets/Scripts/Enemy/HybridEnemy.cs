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
        if (!_isSetUp) return;

        _anim.SetFloat("Speed", _speed);
        _speed = Mathf.Lerp(_speed, _agent.velocity.normalized.magnitude, Time.deltaTime * animTransSpeed);

        if (_agent.isActiveAndEnabled)
            _agent.SetDestination(GameManager.instance.player.transform.position);

        FacePlayer();

        if (!_isAttacking)
        {
            _isAttacking = true;
            if (_inAttackRange)
                EnemyManager.instance.QueueAttack(Melee);
            else
                EnemyManager.instance.QueueAttack(Shoot);            
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

        yield return new WaitForSeconds(_secondaryAttackStats.rate);
        _isAttacking = false;
    }
}
