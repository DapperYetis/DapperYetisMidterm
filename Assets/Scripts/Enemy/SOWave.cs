using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave #", menuName = "Wave")]
public class SOWave : ScriptableObject
{
    [SerializeField]
    private bool _ignoreSpawnBudget;
    public bool ignoreSpawnBudget => _ignoreSpawnBudget;
    [SerializeField]
    private GameObject _enemyType;
    public GameObject enemyType => _enemyType;
    [SerializeField]
    private float _spawnInterval;
    public float spawnInterval => _spawnInterval;
    [SerializeField]
    private int _maxEnemyCount;
    public int maxEnemyCount => _maxEnemyCount;

    public float WaveTime() => _maxEnemyCount * _spawnInterval;

    public static int CalcSpawnCost(EnemyAI enemy, int count) => (int)enemy.spawnCost * count;
}
