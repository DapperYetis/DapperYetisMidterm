using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Explosion : Projectile
{
    private List<IBuffable> _previouslyHit = new();

    protected override void Start()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        StartCoroutine(DoExplode());
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IBuffable>(out var buffable) && !_previouslyHit.Contains(buffable))
        {
            _previouslyHit.Add(buffable);
            buffable.Damage(_stats.directDamage);
            OnHit?.Invoke(this, buffable);
        }
    }

    protected virtual IEnumerator DoDestroy()
    {
        yield return new WaitForSeconds(_stats.lifetime);
        Destroy(gameObject);
    }

    protected virtual IEnumerator DoExplode()
    {
        Destroy(gameObject, _stats.secondaryLifetime);
        _stats.directDamage = _stats.secondaryDamage;
        while(true)
        {
            transform.localScale += Vector3.one * (stats.fxIntesity * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual void OnDestroy()
    {
        StopAllCoroutines();
    }
}
