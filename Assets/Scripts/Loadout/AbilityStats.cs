using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct AbilityStats
{
    public StatChangeType changeType;
    public GameObject prefab;
    [Range(0, 1)]
    public float critChance;
    public float directDamage;
    public float secondaryDamage;
    public float cooldown;
    public float lifetime;
    public float secondaryLifetime;
    public float speed;
    public float fxIntesity;
    public bool canInterrupt;
    public List<SOBuff> selfBuffs;
    public List<SOBuff> targetBuffs;

    public static AbilityStats operator +(AbilityStats s1, AbilityStats s2)
    {
        s1.critChance += s2.critChance;
        s1.directDamage += s2.directDamage;
        s1.secondaryDamage += s2.secondaryDamage;
        s1.cooldown += s2.cooldown;
        if (s1.cooldown < 0.1f)
        {
            s1.cooldown = 0.1f;
        }
        s1.lifetime += s2.lifetime;
        s1.secondaryLifetime += s2.secondaryLifetime;
        s1.speed += s2.speed;
        s1.fxIntesity += s2.fxIntesity;

        return s1;
    }

    public static AbilityStats operator *(AbilityStats s1, AbilityStats s2)
    {
        s1.critChance *= s2.critChance;
        s1.directDamage *= s2.directDamage;
        s1.secondaryDamage *= s2.secondaryDamage;
        s1.cooldown *= s2.cooldown;

        s1.lifetime *= s2.lifetime;
        s1.secondaryLifetime *= s2.secondaryLifetime;
        s1.speed *= s2.speed;
        s1.fxIntesity *= s2.fxIntesity;

        return s1;
    }


    public static implicit operator EnemyAttackStats(AbilityStats stats)
    {
        return new EnemyAttackStats
        {
            prefab = stats.prefab,
            damage = stats.directDamage,
            rate = stats.cooldown,
            lifetime = stats.lifetime,
            selfBuffs = stats.selfBuffs,
            targetBuffs = stats.targetBuffs,
            speed = stats.speed
        };
    }
}
