using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffect : MonoBehaviour
{
    protected SOItem _item;
    protected int _stacks;
    public int stacks => _stacks;
    [SerializeField]
    protected float _itemEndTime = 0.1f;

    public virtual void SetUp(SOItem item)
    {
        _stacks = 0;
        _item = item;
    }

    protected virtual void DestroyEffect()
    {
        Destroy(gameObject);
    }

    protected virtual void EffectRemoveAction() { }

    protected virtual void EffectAddAction() { }

    protected virtual void EffectActiveAction() { }

    public virtual void AddStacks(int amount)
    {
        _stacks += amount;
    }

    public void RemoveStacks(int amount)
    {
        _stacks -= amount;

        EffectRemoveAction();
        if (_stacks <= 0)
            Invoke(nameof(DestroyEffect), _itemEndTime);
    }
}
