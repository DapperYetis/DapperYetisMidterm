using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DirtProjectile : Projectile
{
    protected Rigidbody _rb;
    protected EnemyAI _target;

    protected override void Start()
    {
        base.Start();
        _rb = GetComponent<Rigidbody>();
        _target = EnemyManager.instance.GetEnemyListSize() > 0 ? (from enemy in EnemyManager.instance.enemies orderby (enemy.transform.position - transform.position).magnitude / Vector3.Dot(enemy.transform.position - transform.position.normalized, transform.forward) ascending select enemy).First() : null;
    }

    private void FixedUpdate()
    {
        if(_target != null)
            _rb.velocity += (_target.transform.position - transform.position).normalized * (_stats.speed * Time.fixedDeltaTime);
    }
}
