using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;

public class DamageTrigger : MonoBehaviour
{
    [SerializeField] private float damage;
    //[HideInInspector]
    //public UnityEvent<Projectile, IDamageable> OnHit;

    [SerializeField]
    private bool _piercing;
    private List<IDamageable> _previouslyHit = new();

    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        Assert.IsNotNull(collider);
        Assert.IsTrue(collider.isTrigger);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
        {
            //damageable.Damage(damage);
            _previouslyHit.Add(damageable);
            //damageable.Damage(_stats.directDamage);
        }


        //IDamageable damageable = other.GetComponent<IDamageable>();
        //if (damageable != null)
        //{
        //    damageable.Damage(damage);
        //}
    }
}
