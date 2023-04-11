using System.Collections.Generic;
using UnityEngine;

public struct LevelWaveInfo
{
    public int waveCount;
    public List<Transform> spawnPoints;
    public List<WaveStats> waves;

    public LevelWaveInfo(int waveCount, List<Transform> spawnPoints, List<WaveStats> waves)
    {
        this.waveCount = waveCount;
        this.spawnPoints = spawnPoints;
        this.waves = waves;
    }
}