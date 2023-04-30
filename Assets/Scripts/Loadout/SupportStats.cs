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
        // Primary
        s1.useRatePrimary += s2.useRatePrimary;
        s1.useCountPrimary += s2.useCountPrimary;
        s1.distancePrimary += s2.distancePrimary;
        s1.damagePrimary += s2.damagePrimary;

        // Secondary
        s1.useRateSecondary += s2.useRateSecondary;
        s1.useCountSecondary += s2.useCountSecondary;
        s1.distanceSecondary += s2.distanceSecondary;
        s1.damageSecondary += s2.damageSecondary;

        return s1;
    }
}
