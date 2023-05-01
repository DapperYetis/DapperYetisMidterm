using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : Projectile
{
    [SerializeField]
    private bool _piercing;
    private List<IDamageable> _previouslyHit = new();

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
        {
            _previouslyHit.Add(damageable);
            damageable.Damage(_stats.directDamage);
        }

        if (!_piercing)
            Destroy(gameObject);
    }
}
