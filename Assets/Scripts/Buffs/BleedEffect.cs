using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BleedEffect : BuffRemoveEffect
{
    protected override void Effect()
    {
        _target.Damage(_buff.abilityMods.directDamage);
        Debug.Log("Bleed damage");
    }
}
