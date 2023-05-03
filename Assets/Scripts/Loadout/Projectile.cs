using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected AbilityStats _stats;
    public AbilityStats stats => _stats;

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
            damageable.Damage(_stats.directDamage);
        }
        if(other.gameObject.TryGetComponent<IBuffable>(out var buffable) && _stats.targetBuffs != null)
        {
            var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToList();
            buffable.AddBuffs(buffs);
        }

        Destroy(gameObject);
    }

}
