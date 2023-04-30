using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : Projectile
{
    [SerializeField]
    private bool _isAOE;
    private bool _exploding;
    private List<IDamageable> _previouslyHit = new();

    protected override void Start()
    {
        StartCoroutine(DoDestroy());
        GetComponent<Rigidbody>().velocity = transform.forward * _stats.speed;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        if(!_exploding)
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.Damage(_stats.directDamage);
            }
            StartCoroutine(DoExplode());
        }
        else if(other.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
        {
            _previouslyHit.Add(damageable);
            damageable.Damage(_stats.directDamage);
        }

        if (!_isAOE)
            Destroy(gameObject);
    }

    protected virtual IEnumerator DoDestroy()
    {
        yield return new WaitForSeconds(_stats.lifetime);
        if (!_exploding)
            Destroy(gameObject);
    }

    protected virtual IEnumerator DoExplode()
    {
        _exploding = true;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        Destroy(gameObject, _stats.secondaryLifetime);
        while(true)
        {
            transform.localScale += Vector3.one * _stats.fxIntesity;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }
}
