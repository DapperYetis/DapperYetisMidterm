using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyMeleeSlash : MonoBehaviour
{
    [SerializeField]
    protected MeleeEnemy _enemy;
    private List<IDamageable> _previouslyHit = new();
    protected Collider slash;

    private void Start()
    {
        slash = GetComponent<SphereCollider>();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            _previouslyHit.Add(damageable);
            damageable.Damage(_enemy.primaryAttackStats.damage);
        }

        slash.enabled = false;
    }
}
