using UnityEngine;

[System.Serializable]
public struct AbilityStats
{
    public GameObject prefab;
    public float damage;
    public float cooldown;
    public float lifetime;
    public float speed;


    public static AbilityStats operator +(AbilityStats s1, AbilityStats s2)
    {
        AbilityStats stats = new();

        stats.prefab = s1.prefab;
        stats.damage = s1.damage + s2.damage;
        stats.cooldown = s1.cooldown + s2.cooldown;
        stats.lifetime = s1.lifetime + s2.lifetime;
        stats.speed = s1.speed + s2.speed;

        return stats;
    }
}
