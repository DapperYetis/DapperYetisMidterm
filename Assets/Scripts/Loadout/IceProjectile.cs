using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IceProjectile : Projectile
{
    [SerializeField]
    private bool _piercing;
    private List<IDamageable> _previouslyHit = new();

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
            if (_hasCrit)
                damageable.Damage(_stats.directDamage, buffs);
            else
                damageable.Damage(_stats.directDamage);
            _previouslyHit.Add(damageable);

            OnHit?.Invoke(this, damageable);
        }

        if (!_piercing)
            Destroy(gameObject);
    }
}
