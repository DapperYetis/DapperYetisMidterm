using UnityEngine;

[System.Serializable]
public struct AbilityStats
{
    public GameObject prefab;
    public float secondaryDamage;
    public float directDamage;
    public float cooldown;
    public float lifetime;
    public float secondaryLifetime;
    public float speed;
    public float fxIntesity;


    public static AbilityStats operator +(AbilityStats s1, AbilityStats s2)
    {
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
}
