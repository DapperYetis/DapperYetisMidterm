using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fork : ItemEffect
{
    private void Start()
    {
        GameManager.instance.player.OnHit.AddListener(Stab);
    }

    protected void Stab(Projectile projectile, IBuffable buffable)
    {
        if(projectile.hasCrit)
        {
            for(int i = 0; i < _item.attackStats.primaryAbility.targetBuffs.Count; ++i)
                buffable.AddBuff(_item.attackStats.primaryAbility.targetBuffs[i], _stacks);
        }
    }
}
