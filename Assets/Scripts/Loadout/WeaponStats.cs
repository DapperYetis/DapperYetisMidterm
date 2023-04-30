using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    public AbilityStats primaryAbility;
    public AbilityStats secondaryAbility;



    public static WeaponStats operator +(WeaponStats s1, WeaponStats s2)
    {
        s1.primaryAbility += s2.primaryAbility;
        s1.secondaryAbility += s2.secondaryAbility;

        return s1;
    }
}
