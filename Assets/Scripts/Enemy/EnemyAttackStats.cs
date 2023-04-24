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


    public static EnemyAttackStats operator+(EnemyAttackStats s1, EnemyAttackStats s2)
    {
        EnemyAttackStats stats = new();

        stats.positions = s1.positions;
        stats.damage = s1.damage + s2.damage;
        stats.rate = s1.rate + s2.rate;
        stats.lifetime = s1.lifetime + s2.lifetime;
        stats.variance = s1.variance + s2.variance;
        stats.prefab = s1.prefab;
        stats.speed = s1.speed + s2.speed;

        return stats;
    }

    public static implicit operator AbilityStats(EnemyAttackStats stats)
    {
        return new AbilityStats
        {
            prefab = stats.prefab,
            damage = stats.damage,
            cooldown = stats.rate,
            lifetime = stats.lifetime,
            speed = stats.speed
        };
    }
}
