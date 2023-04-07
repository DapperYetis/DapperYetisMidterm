using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PlayerStats
{
    [Header("---Movement---")]
    
    public float walkSpeed;
    public float runSpeed;
    public AnimationCurve accelerationRate;
    [Range(0, 5)]
    public float acclerationTime;


    [Space(20), Header("---Jumping---")]

    public float jumpHeight;
    public int jumpCountMax;
    public float gravityAcceleration;
}