using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected SOWave _bossWave;
    [SerializeField]
    protected Transform _spawnPoint;
    protected int _cost = 1000;

    public bool Interact()
    {
        EnemyManager.SpawnWave(_bossWave, _spawnPoint);
        transform.parent.gameObject.SetActive(false);
        return true;
    }

    public bool CanInteract()
    {
        return true;
    }

    public int GetCost() => _cost;
}
