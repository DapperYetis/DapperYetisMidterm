using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    public AbilityStats primaryAbility;
    public AbilityStats secondaryAbility;



    public static WeaponStats operator +(WeaponStats s1, WeaponStats s2)
    {
        WeaponStats stats = new();

        stats.primaryAbility = s1.primaryAbility + s2.primaryAbility;
        stats.secondaryAbility = s1.secondaryAbility + s2.secondaryAbility;

        return stats;
    }
}
