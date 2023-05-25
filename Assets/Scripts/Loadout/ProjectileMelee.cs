using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProjectileMelee : Projectile
{
    protected List<IBuffable> _previouslyHit = new();

    protected override void Start()
    {

        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    protected override void OnTriggerStay(Collider other)
    {
        if (other.isTrigger) return;

        if (other.gameObject.TryGetComponent<IBuffable>(out var buffable) && !_previouslyHit.Contains(buffable))
        {
            if(_stats.targetBuffs != null)
            {
                var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
                if (buffs.Length > 0)
                    buffable.Damage(_stats.directDamage, buffs);
                else
                    buffable.Damage(_stats.directDamage);
            }
            else
                buffable.Damage(_stats.directDamage);

            StartCoroutine(TrackHit(buffable));
        }
    }

    private IEnumerator TrackHit(IBuffable target)
    {
        _previouslyHit.Add(target);
        yield return new WaitForSeconds(10);
        OnHit?.Invoke(this, target);

        yield return new WaitForSeconds(_stats.cooldown);
        _previouslyHit.Remove(target);
    }

    private void OnDisable()
    {
        _previouslyHit.Clear();
    }
}
