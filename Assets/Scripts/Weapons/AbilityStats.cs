using UnityEngine;

[System.Serializable]
public struct AbilityStats
{
    public GameObject prefab;
    public float damage;
    public float directDamage;
    public float cooldown;
    public float lifetime;
    public float secondaryLifetime;
    public float speed;
    public float fxIntesity;


    public static AbilityStats operator +(AbilityStats s1, AbilityStats s2)
    {
        AbilityStats stats = new();

        stats.prefab = s1.prefab;
        stats.damage = s1.damage + s2.damage;
        stats.directDamage = s1.directDamage + s2.directDamage;
        stats.cooldown = s1.cooldown + s2.cooldown;
        if (stats.cooldown < 0.1f)
        {
            stats.cooldown = 0.1f;
        }
        stats.lifetime = s1.lifetime + s2.lifetime;
        stats.secondaryLifetime = s1.secondaryLifetime + s2.secondaryLifetime;
        stats.speed = s1.speed + s2.speed;
        stats.fxIntesity = s1.fxIntesity+ s2.fxIntesity;

        return stats;
    }
}
