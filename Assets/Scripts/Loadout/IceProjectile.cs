using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceProjectile : Projectile
{
    [SerializeField]
    private bool _piercing;
    private List<IBuffable> _previouslyHit = new();

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IBuffable>(out var buffable) && !_previouslyHit.Contains(buffable))
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
            if (_hasCrit)
                buffable.Damage(_stats.directDamage, buffs);
            else
                buffable.Damage(_stats.directDamage);
            _previouslyHit.Add(buffable);

            OnHit?.Invoke(this, buffable);
        }

        if (!_piercing)
            Destroy(gameObject);
    }
}
