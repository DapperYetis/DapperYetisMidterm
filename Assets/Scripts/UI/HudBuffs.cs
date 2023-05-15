using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HudBuffs : MonoBehaviour
{
    [SerializeField] GameObject _buffsUI;
    [SerializeField] GameObject _buffLayout;


    private Dictionary<SOBuff, BuffHudItems> _buffList = new();
    private BuffHudItems buff;

    private void Start()
    {
        GameManager.instance.player.OnBuffAdd.AddListener(AddBuff);
        GameManager.instance.player.OnBuffStackIncrease.AddListener(IncreaseStack);
        GameManager.instance.player.OnBuffStackDecrease.AddListener(DecreaseStack);
        GameManager.instance.player.OnBuffRemove.AddListener(RemoveBuff);
    }

    private void AddBuff(SOBuff buffToAdd)
    {
        if (!_buffList.ContainsKey(buffToAdd))
        {
            buff = Instantiate(_buffsUI, _buffLayout.transform).GetComponent<BuffHudItems>();
            _buffList.Add(buffToAdd, buff);
            buff.SetBuffUI(buffToAdd.icon);
        }
    }
    private void RemoveBuff(SOBuff buffToRemove)
    {
        Destroy(buff.GameObject());
        _buffList.Remove(buffToRemove);
    }
    private void IncreaseStack(SOBuff toChange, int toAdd)
    {
        if (_buffList.ContainsKey(toChange))
            _buffList[toChange].SetBuffStack(toAdd);
    }
    private void DecreaseStack(SOBuff buffToChange, int toSubtract)
    {
        _buffList[buffToChange].SetBuffStack(-toSubtract);
    }

}
