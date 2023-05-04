using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffEffect : MonoBehaviour
{
    protected IBuffable _target;
    protected SOBuff _buff;
    protected int _stacks;
    public int stacks => _stacks;
    [SerializeField]
    protected float _buffEndTime = 0.1f;

    public virtual void SetUp(IBuffable target, SOBuff buff)
    {
        _stacks = 0;
        _target = target;
        _buff = buff;
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
        Debug.Log(_stacks);
        EffectRemoveAction();
        if (_stacks <= 0)
            Invoke(nameof(DestroyEffect), _buffEndTime);
    }
}
