using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDrops : MonoBehaviour
{
    [SerializeField]
    private int _xp;
    [SerializeField]
    private int _currency;
    [SerializeField]
    private int _score = 10;
    private float _difficulty => EnemyManager.instance.scaleFactor;

    public void Drop()
    {
        GameManager.instance.player.inventory.AddCurrency(Mathf.FloorToInt(_currency * _difficulty));
        GameManager.instance.player.inventory.AddXP(Mathf.FloorToInt(_xp * _difficulty));
        GameManager.instance.AddToScore(_score);
    }
}
