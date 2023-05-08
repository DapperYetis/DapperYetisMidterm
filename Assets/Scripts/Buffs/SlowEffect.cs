using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowEffect : BuffEffect
{
    public override void SetUp(IBuffable target, SOBuff buff)
    {
        base.SetUp(target, buff);

        InvokeRepeating(nameof(EffectActiveAction), _buff.abilityMods.cooldown / 2f, _buff.abilityMods.cooldown);
    }
}
