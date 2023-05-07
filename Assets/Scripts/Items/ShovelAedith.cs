using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelAedith : ItemEffect
{
    [SerializeField]
    private GameObject _projectilePrefab;

    private void Start()
    {
        GameManager.instance.player.OnHit.AddListener(Fire);
    }

    protected void Fire(Projectile projectile, IDamageable damageable)
    {
        DirtProjectile fired = Instantiate(_projectilePrefab, GameManager.instance.player.transform.position, GameManager.instance.player.transform.rotation).GetComponent<DirtProjectile>();
        fired.SetStats(_item.attackStats.primaryAbility);
    }
}
