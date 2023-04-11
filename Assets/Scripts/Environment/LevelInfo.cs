using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelInfo : MonoBehaviour
{
    [SerializeField]
    private Transform _playerSpawn;
    private List<Transform> _wavePoints;
    [SerializeField]
    private List<SOWave> _waveConfigurations;
    [SerializeField]
    private int _waveCount;

    private void Start()
    {
        GameManager.instance.level = this;
        FindSpawnPoints();
        EnemyManager.instance.SetWaves(new(_waveCount, _wavePoints, (from spawnPoint in _waveConfigurations select spawnPoint.stats).ToList()));
        ResetMap();
    }

    private void FindSpawnPoints()
    {
        _wavePoints = (from go in GameObject.FindGameObjectsWithTag("EnemySpawnPoint") select go.transform).ToList();
    }

    public void ResetMap()
    {
        if (GameManager.instance.playerMovement == null) return;

        GameManager.instance.playerMovement.enabled = false;
        GameManager.instance.player.transform.position = _playerSpawn.position;
        GameManager.instance.playerMovement.enabled = true;
    }
}
