using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct EnemyAttackStats
{
    public Transform[] positions;
    public float damage;
    public float rate;
    public float lifetime;
    public float variance;
    public GameObject prefab;
    public float speed;


    public static EnemyAttackStats operator +(EnemyAttackStats s1, EnemyAttackStats s2)
    {
        s1.damage += s2.damage;
        s1.rate += s2.rate;
        s1.lifetime += s2.lifetime;
        s1.variance += s2.variance;
        s1.speed += s2.speed;

        return s1;
    }

    public static implicit operator AbilityStats(EnemyAttackStats stats)
    {
        return new AbilityStats
        {
            prefab = stats.prefab,
            directDamage = stats.damage,
            cooldown = stats.rate,
            lifetime = stats.lifetime,
            speed = stats.speed
        };
    }
}
