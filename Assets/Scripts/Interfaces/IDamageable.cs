using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damage, (SOBuff buff, int amount)[] buffs = null);
    public void Heal(float health, bool silent = false);

    public float GetHealthMax();
    public float GetHealthCurrent();
}
