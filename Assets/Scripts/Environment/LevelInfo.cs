using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField]
    private Transform _playerSpawn;
    [SerializeField]
    private List<EnemySpawnStats> _spawnPoints;

    private void Start()
    {
        GameManager.instance.level = this;
        EnemyManager.instance.SetSpawnPoints(_spawnPoints);
        ResetMap();
    }

    public void ResetMap()
    {
        if (GameManager.instance.playerMovement == null) return;

        GameManager.instance.playerMovement.enabled = false;
        GameManager.instance.player.transform.position = _playerSpawn.position;
        GameManager.instance.playerMovement.enabled = true;
    }
}
