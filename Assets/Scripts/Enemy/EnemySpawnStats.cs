using UnityEngine;
using UnityEngine.UIElements;

[System.Serializable]
public struct EnemySpawnStats
{
    public GameObject prefab;
    public float spawnInterval;
    public Transform spawnPosition;
}