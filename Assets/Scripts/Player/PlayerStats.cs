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

    public static PlayerStats operator +(PlayerStats s1, PlayerStats s2)
    {
        PlayerStats stats = new();


        stats.walkSpeed = s1.walkSpeed + s2.walkSpeed;
        stats.sprintMultiplier = s1.sprintMultiplier + s2.sprintMultiplier;
        stats.accelerationRate = s1.accelerationRate;
        stats.accelerationTime = s1.accelerationTime;

        stats.jumpHeightMin = s1.jumpHeightMin + s2.jumpHeightMin;
        stats.jumpHeightMax = s1.jumpHeightMax + s2.jumpHeightMax;
        stats.jumpInputTime = s1.jumpInputTime;
        stats.jumpCountMax = s1.jumpCountMax + s2.jumpCountMax;
        stats.gravityAcceleration = s1.gravityAcceleration;

        stats.healthMax = s1.healthMax + s2.healthMax;


        return stats;
    }
}