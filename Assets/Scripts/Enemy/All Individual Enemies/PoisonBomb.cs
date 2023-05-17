using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : RangedEnemy
{
    [SerializeField] AudioSource _audFuse;
    [SerializeField] float _closeEnough = 10;

    protected override void Update()
    {
        if (!_isSetUp) return;

        _anim.SetFloat("Speed", _speed);
        _speed = Mathf.Lerp(_speed, _agent.velocity.normalized.magnitude, Time.deltaTime * _animTransSpeed);

        if (_agent.isActiveAndEnabled && _HPCurrent > 0)
        {
            Movement();
        }

        CheckBuffs();
    }

    protected override void Movement()
    {
        base.Movement();

        if ((GameManager.instance.player.transform.position - transform.position).magnitude < _closeEnough)
        {
            _aud.PlayOneShot(_audGettingClose[Random.Range(0, _audGettingClose.Length)], _audGettingCloseVol);
        }
    }

    protected override IEnumerator FireShot()
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
        Destroy(this);
    }
}
