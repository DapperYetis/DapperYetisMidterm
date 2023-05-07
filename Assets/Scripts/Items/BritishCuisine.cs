using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BritishCuisine : ItemEffect
{
    private void Start()
    {
        GameManager.instance.player.OnHit.AddListener(Heal);
    }

    protected void Heal(Projectile projectile, IDamageable damageable)
    {
        GameManager.instance.player.Heal(_item.attackStats.primaryAbility.directDamage * _stacks);
    }
}
