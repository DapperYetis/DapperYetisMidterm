using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    [SerializeField]
    private GameObject _explosionPrefab;

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (_explosionPrefab)
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity).GetComponent<Projectile>().SetStats(_stats);

        Destroy(gameObject);
    }
}
