using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField]
    private AbilityStats _stats;
    public AbilityStats stats => _stats;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _stats.lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * _stats.speed;
    }

    public virtual void SetStats(AbilityStats stats)
    {
        _stats = stats;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.Damage(_stats.damage);
        }
        else if (other.transform.gameObject.layer == 8 || other.transform.gameObject.layer == 9)
            Destroy(other);

        Destroy(gameObject);
    }

}
