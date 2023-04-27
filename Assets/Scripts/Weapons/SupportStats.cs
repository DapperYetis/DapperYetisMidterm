using UnityEngine;

[System.Serializable]
public struct SupportStats
{
    // Primary
    public float useRatePrimary;
    public int useCountPrimary;
    public float distancePrimary;
    public float damagePrimary;

    // Secondary
    public float useRateSecondary;
    public int useCountSecondary;
    public float distanceSecondary;
    public float damageSecondary;
    

    public static SupportStats operator +(SupportStats s1, SupportStats s2)
    {
        SupportStats stats = new();

        // Primary
        stats.useRatePrimary = s1.useRatePrimary + s2.useRatePrimary;
        stats.useCountPrimary = s1.useCountPrimary + s2.useCountPrimary;
        stats.distancePrimary = s1.distancePrimary + s2.distancePrimary;
        stats.damagePrimary = s1.damagePrimary + s2.damagePrimary;

        // Secondary
        stats.useRateSecondary= s1.useRateSecondary+ s2.useRateSecondary;
        stats.useCountSecondary = s1.useCountSecondary + s2.useCountSecondary;
        stats.distanceSecondary = s1.distanceSecondary + s2.distanceSecondary;
        stats.damageSecondary = s1.damageSecondary + s2.damageSecondary;

        return stats;
    }
}
