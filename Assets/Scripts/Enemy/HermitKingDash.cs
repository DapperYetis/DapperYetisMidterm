using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class HermitKingDash : MonoBehaviour
{
    [SerializeField]
    private HermitKingBoss _enemy;
    private List<IDamageable> _previouslyHit = new();
    private Collider dash;

    private void Start()
    {
        dash = GetComponent<SphereCollider>();
    }

    private void OnEnable()
    {
        Invoke(nameof(EndDash), _enemy.dashAttackStats.lifetime);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            _previouslyHit.Add(damageable);
            damageable.Damage(_enemy.dashAttackStats.damage);
        }
    }

    public void EndDash()
    {
        enabled = false;
    }
}
