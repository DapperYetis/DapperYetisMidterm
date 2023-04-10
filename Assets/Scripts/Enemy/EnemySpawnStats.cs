using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct EnemySpawnStats
{
    public GameObject spawnPointPrefab;
    public float spawnInterval;
    public Transform spawnPosition;
}