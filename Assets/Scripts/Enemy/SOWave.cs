using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave #", menuName = "Wave")]
public class SOWave : ScriptableObject
{
    [SerializeField]
    private WaveStats _stats;
    public WaveStats stats => _stats;

    public float WaveTime() => _stats.maxEnemyCount * _stats.spawnInterval;
}
