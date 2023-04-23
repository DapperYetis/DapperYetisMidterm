using UnityEngine;

[System.Serializable]
public struct WeaponStats
{
    [SerializeField]
    private AbilityStats _primaryAbility;
    public AbilityStats primaryAbility => _primaryAbility;
    [SerializeField]
    private AbilityStats _secondaryAbility;
    public AbilityStats secondaryAbility => _secondaryAbility;



    public static WeaponStats operator +(WeaponStats s1, WeaponStats s2)
    {
        WeaponStats stats = new();

        stats._primaryAbility = s1._primaryAbility + s2._primaryAbility;
        stats._secondaryAbility = s1._secondaryAbility + s2._secondaryAbility;

        return stats;
    }
}
