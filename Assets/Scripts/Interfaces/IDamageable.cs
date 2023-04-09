using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(float damage);
    public void Heal(float health);

    public float GetHealthMax();
    public float GetHealthCurrent();
}
