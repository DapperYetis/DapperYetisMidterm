using UnityEngine;

[System.Serializable]
public struct EnemyStats
{
    public float HPMax;
    public int facePlayerSpeed;
    public float speed;
    public float acceleration;

    public static EnemyStats operator +(EnemyStats s1, EnemyStats s2)
    {
        s1.HPMax += s2.HPMax;
        s1.facePlayerSpeed += s2.facePlayerSpeed;
        s1.speed += s2.speed;
        s1.acceleration += s2.acceleration;

        return s1;
    }

    public static EnemyStats operator *(EnemyStats s1, EnemyStats s2)
    {
        s1.HPMax *= s2.HPMax;
        s1.facePlayerSpeed *= s2.facePlayerSpeed;
        s1.speed *= s2.speed;
        s1.acceleration *= s2.acceleration;

        return s1;
    }

    public static EnemyStats operator *(float scalar, EnemyStats s1)
    {
        s1.HPMax *= scalar;
        s1.facePlayerSpeed = (int)(s1.facePlayerSpeed * scalar);
        s1.speed *= scalar;
        s1.acceleration *= scalar;

        return s1;
    }
}