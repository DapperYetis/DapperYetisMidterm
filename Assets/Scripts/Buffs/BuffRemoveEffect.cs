using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffRemoveEffect : MonoBehaviour
{
    protected IBuffable _target;
    protected SOBuff _buff;

    public virtual void SetUp(IBuffable target, SOBuff buff)
    {
        _target = target;
        _buff = buff;


        Invoke(nameof(DestroyEffect), 0.1f);
        Effect();
    }

    protected virtual void DestroyEffect()
    {
        Destroy(gameObject);
    }

    protected abstract void Effect();
}
