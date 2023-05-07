using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour, IInteractable
{
    [SerializeField]
    protected SOWave _bossWave;
    [SerializeField]
    protected Transform _spawnPoint;
    [SerializeField]
    protected int _cost = 1000;

    private void Start()
    {
        _cost = (int)(_cost * EnemyManager.instance.scaleFactor);
    }

    public bool Interact()
    {
        EnemyManager.SpawnEnemy(_bossWave, _spawnPoint.position);
        transform.parent.gameObject.SetActive(false);
        return true;
    }

    public bool CanInteract()
    {
        return true;
    }

    public int GetCost() => _cost;
}
