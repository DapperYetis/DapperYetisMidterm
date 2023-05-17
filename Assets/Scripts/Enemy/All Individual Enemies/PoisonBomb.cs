using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : MeleeEnemy
{
    protected override void Movement()
    {
        FacePlayer();
        _agent.SetDestination(GameManager.instance.player.transform.position);

        if ((GameManager.instance.player.transform.position - transform.position).magnitude < 10)
        {
            _aud.PlayOneShot(_audGettingClose[Random.Range(0, _audGettingClose.Length)], _audGettingCloseVol);
        }
    }


}
