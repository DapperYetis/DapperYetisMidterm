using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave #", menuName = "Wave")]
public class SOWave : ScriptableObject
{
    [SerializeField]
    private GameObject _spawnPointPrefab;
    public GameObject spawnPointPrefab => _spawnPointPrefab;
    [SerializeField]
    private float _spawnInterval;
    public float spawnInterval => _spawnInterval;
    [SerializeField]
    private int _maxEnemyCount;
    public int maxEnemyCount => _maxEnemyCount;

    public float WaveTime() => _maxEnemyCount * _spawnInterval;
}
