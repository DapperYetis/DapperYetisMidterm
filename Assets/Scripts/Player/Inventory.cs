using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private Dictionary<SOItem, int> _items;
    public Dictionary<SOItem, int> items => _items;

    private int _currentLevel;
    public int currentLevel => _currentLevel;
    private int _currentXP;
    public int currentXP => _currentXP;
    private int _currency;
    public int currency => _currency;

    [HideInInspector]
    public UnityEvent<int> OnLevelChange;
    [HideInInspector]
    public UnityEvent<int> OnXPChange;
    [HideInInspector]
    public UnityEvent<int> OnCurrencyChange;
    [HideInInspector]
    public UnityEvent<SOItem> OnItemsChange;

    private void Start()
    {
        _items = new();
    }

    public void AddXP(int amount)
    {
        if (amount <= 0) return;

        _currentXP += amount;
        // Level change
        OnXPChange.Invoke(amount);
    }

    private void LevelUp(int levelCount)
    {
        if (levelCount <= 0) return;

        _currentLevel += levelCount;
        OnLevelChange.Invoke(levelCount);
    }

    public void AddCurrency(int amount)
    {
        if (amount <= 0) return;

        _currency += amount;
        OnCurrencyChange.Invoke(amount);
    }

    public void AddItem(SOItem item)
    {
        if (_items.ContainsKey(item))
            ++_items[item];
        else
            _items.Add(item, 1);

        Debug.Log($"Item: {item.name}\tCount: {_items[item]}");
        OnItemsChange.Invoke(item);
    }

    public void Spend(int cost)
    {
        _currency -= cost;
        OnCurrencyChange.Invoke(-cost);
    }

    public void ResetPlayer()
    {
        _currency = 0;
        _items.Clear();
        _currentLevel = 0;
    }
}
