using UnityEngine;

[System.Serializable]
public struct EnemyStats
{
    public float HPMax;
    public int facePlayerSpeed;

    public static EnemyStats operator +(EnemyStats s1, EnemyStats s2)
    {
        s1.HPMax += s2.HPMax;
        s1.facePlayerSpeed += s2.facePlayerSpeed;

        return s1;
    }
}