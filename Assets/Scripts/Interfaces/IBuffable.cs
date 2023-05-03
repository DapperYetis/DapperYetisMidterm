using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffable : IDamageable
{
    public List<SOBuff> GetBuffs();
    public void AddBuff(SOBuff buff, int amount = 0);
    public void AddBuffs(List<(SOBuff buff, int count)> buffCounts);
    public int GetStackCount(SOBuff buff);
    public bool RemoveBuff(SOBuff buff);
}
