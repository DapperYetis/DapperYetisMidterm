using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damage(int damage);
    public void Heal(int health);

    public float GetHealthMax();
    public float GetHealthCurrent();
}
