using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [Header("---Movement---")]

    public float walkSpeed;
    public float sprintMultiplier;
    public AnimationCurve accelerationRate;
    [Range(0, 5)]
    public float accelerationTime;


    [Space(20), Header("---Jumping---")]

    public float jumpHeightMin;
    public float jumpHeightMax;
    public float jumpInputTime;
    public int jumpCountMax;
    public float gravityAcceleration;

    [Space(20), Header("---Combat---")]
    public float healthMax;
    public StatChangeType changeType;

    public static PlayerStats operator +(PlayerStats s1, PlayerStats s2)
    {
        s1.walkSpeed += s2.walkSpeed;
        if (s1.walkSpeed < 0)
            s1.walkSpeed = 0;
        s1.sprintMultiplier += s2.sprintMultiplier;

        s1.jumpHeightMin += s2.jumpHeightMin;
        s1.jumpHeightMax += s2.jumpHeightMax;
        s1.jumpCountMax += s2.jumpCountMax;

        s1.healthMax += s2.healthMax;


        return s1;
    }

    public static PlayerStats operator *(PlayerStats s1, PlayerStats s2)
    {
        s1.walkSpeed *= s2.walkSpeed;
        s1.sprintMultiplier *= s2.sprintMultiplier;

        s1.jumpHeightMin *= s2.jumpHeightMin;
        s1.jumpHeightMax *= s2.jumpHeightMax;
        s1.jumpCountMax *= s2.jumpCountMax;

        s1.healthMax *= s2.healthMax;


        return s1;
    }

    public static PlayerStats operator *(float scalar, PlayerStats s1)
    {
        s1.walkSpeed *= scalar;
        s1.sprintMultiplier *= scalar;

        s1.jumpHeightMin *= scalar;
        s1.jumpHeightMax *= scalar;
        s1.jumpCountMax = (int)(s1.jumpCountMax * scalar);

        s1.healthMax *= scalar;


        return s1;
    }

    public static implicit operator EnemyStats(PlayerStats stats)
    {
        return new EnemyStats
        {
            HPMax = stats.healthMax,
            facePlayerSpeed = 0,
            speed = stats.walkSpeed,
            acceleration = stats.accelerationRate.keys.Length > 0 ? stats.accelerationRate.keys[^1].value : 0
        };
    }
}