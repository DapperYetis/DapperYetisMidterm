using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public abstract class Projectile : MonoBehaviour
{
    protected AbilityStats _stats;
    public AbilityStats stats => _stats;
    [HideInInspector]
    public UnityEvent<Projectile, IDamageable> OnHit;

    protected virtual void Start()
    {
        Destroy(gameObject, _stats.lifetime);
        GetComponent<Rigidbody>().velocity = transform.forward * _stats.speed;
    }

    public virtual void SetStats(AbilityStats stats)
    {
        _stats = stats;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
            damageable.Damage(_stats.directDamage, buffs);

            OnHit?.Invoke(this, damageable);
        }

        Destroy(gameObject);
    }

}
