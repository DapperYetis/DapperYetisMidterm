using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeSlash : MonoBehaviour
{
    protected AbilityStats _stats;
    public AbilityStats stats => _stats;
    Collider slash;

    private void Start()
    {
        slash = GetComponent<BoxCollider>();
        Destroy(gameObject, _stats.lifetime);
        GetComponent<Rigidbody>().velocity = transform.forward;
    }

    private void Update()
    {
        if (true)
        {
            slash.enabled = true;
        }   
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(_stats.damage);
        }

        slash.enabled = false;
    }
}
