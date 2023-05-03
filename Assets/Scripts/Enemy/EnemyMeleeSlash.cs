using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyMeleeSlash : MonoBehaviour
{
    [SerializeField]
    protected MeleeEnemy _enemy;
    private List<IDamageable> _previouslyHit = new();
    protected Collider slash;

    private void Start()
    {
        slash = GetComponent<Collider>();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            _previouslyHit.Add(damageable);
            damageable.Damage(_enemy.primaryAttackStats.damage);
        }
        if(other.gameObject.TryGetComponent<IBuffable>(out var buffable))
        {
            var buffs = (from buff in _enemy.primaryAttackStats.targetBuffs select (buff, 1)).ToList();
            buffable.AddBuffs(buffs);
        }

        slash.enabled = false;
    }
}
