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

    private void Update()
    {
        WhenDead();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            var buffs = (from buff in _enemy.primaryAttackStats.targetBuffs select (buff, 1)).ToArray();
            _previouslyHit.Add(damageable);
            damageable.Damage(_enemy.primaryAttackStats.damage, buffs);
        }

        slash.enabled = false;
    }

    private void WhenDead()
    {
        if (_enemy.GetHealthCurrent() <= 0)
        {
            slash.enabled = false;
        }
    }
}
 