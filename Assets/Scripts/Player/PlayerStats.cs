﻿using System.Collections;
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
    public float acclerationTime;


    [Space(20), Header("---Jumping---")]

    public float jumpHeightMin;
    public float jumpHeightMax;
    [HideInInspector]
    public float jumpVelocityInitial;
    public float jumpInputTime;
    public int jumpCountMax;
    public float gravityAcceleration;

    [Space(20), Header("---Combat---")]
    public float healthMax;
}