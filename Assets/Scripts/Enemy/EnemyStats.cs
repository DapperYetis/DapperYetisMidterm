using UnityEngine;

[System.Serializable]
public struct EnemyStats
{
    public float HPMax;
    public int facePlayerSpeed;

    public static EnemyStats operator+(EnemyStats s1, EnemyStats s2)
    {
        EnemyStats stats = new();

        stats.HPMax = s1.HPMax + s2.HPMax;
        stats.facePlayerSpeed = s1.facePlayerSpeed + s2.facePlayerSpeed;

        return stats;
    }
}