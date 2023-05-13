using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public abstract class Projectile : MonoBehaviour
{
    protected AbilityStats _stats;
    public AbilityStats stats => _stats;
    [HideInInspector]
    public UnityEvent<Projectile, IBuffable> OnHit;
    [HideInInspector]
    public UnityEvent<Projectile, IDamageable> OnHitNonBuffable;
    protected bool _hasCrit;
    public bool hasCrit => _hasCrit;

    protected virtual void Start()
    {
        Destroy(gameObject, _stats.lifetime);
        GetComponent<Rigidbody>().velocity = transform.forward * _stats.speed;
    }

    public virtual void SetStats(AbilityStats stats)
    {
        _stats = stats;
        if(Random.Range(0,1f) <= _stats.critChance)
        {
            _stats.directDamage *= 1.5f;
            _stats.secondaryDamage *= 1.5f;
            _hasCrit = true;
        }
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IBuffable>(out var buffable))
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
            buffable.Damage(_stats.directDamage, buffs);

            OnHit?.Invoke(this, buffable);
        }
        else if(other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(_stats.directDamage);
            OnHitNonBuffable?.Invoke(this, damageable);
        }

        Destroy(gameObject);
    }

}
