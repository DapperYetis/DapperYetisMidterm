using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : MeleeEnemy
{
    [SerializeField] AudioSource _audFuse;
    [SerializeField] float _closeEnough = 10;

    protected override void Movement()
    {
        FacePlayer();
        _agent.SetDestination(GameManager.instance.player.transform.position);

        if ((GameManager.instance.player.transform.position - transform.position).magnitude < _closeEnough)
        {
            _aud.PlayOneShot(_audGettingClose[Random.Range(0, _audGettingClose.Length)], _audGettingCloseVol);
        }
    }

    protected override void AttackStart()
    {
        _meleeCollider.enabled = true;
    }

    protected override void AttackEnd()
    {
        _meleeCollider.enabled = false;
    }
}
