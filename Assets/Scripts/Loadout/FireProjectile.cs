using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FireProjectile : Projectile
{
    [SerializeField]
    private GameObject _explosionPrefab;

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if(_explosionPrefab != null)
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>().SetStats(_stats);
        else
        {
            if(other.TryGetComponent<IBuffable>(out var buffable))
            {
                buffable.Damage(_stats.directDamage, (from buff in _stats.targetBuffs select (buff, 1)).ToArray());
            }
        }

        Destroy(gameObject);
    }
}
