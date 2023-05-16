using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class BloodEmber : ItemEffect
{
    private void Start()
    {
        EnemyManager.instance.OnEnemyDeath.AddListener(DeadlyHeal);
    }

    protected void DeadlyHeal()
    {
        GameManager.instance.player.Heal((10 + (stacks * 10)));
    }
}