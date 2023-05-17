using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBomb : RangedEnemy
{
    [SerializeField] AudioSource _audFuse;
    [SerializeField] float _closeEnough = 10;
    
    protected override void Movement()
    {
        base.Movement();

        if ((GameManager.instance.player.transform.position - transform.position).magnitude < _closeEnough)
        {
            _aud.PlayOneShot(_audGettingClose[Random.Range(0, _audGettingClose.Length)], _audGettingCloseVol);
        }
    }


}
