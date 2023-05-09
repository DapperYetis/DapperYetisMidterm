using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileMelee : Projectile
{
    protected List<IDamageable> _previouslyHit = new();

    protected override void Start()
    {
        
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
            damageable.Damage(_stats.directDamage, buffs);

            StartCoroutine(TrackHit(damageable));
        }
    }

    private IEnumerator TrackHit(IDamageable target)
    {
        _previouslyHit.Add(target);
        OnHit?.Invoke(this, target);

        yield return new WaitForSeconds(_stats.cooldown);
        _previouslyHit.Remove(target);
    }
}