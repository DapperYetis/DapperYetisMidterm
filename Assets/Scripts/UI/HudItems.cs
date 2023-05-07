using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudItems : MonoBehaviour
{
    [SerializeField] GameObject _layoutArea;
    [SerializeField] GameObject _itemUI;

    private Dictionary<SOItem, ItemHudItem> _itemHud = new();


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.player.inventory.OnItemsChange.AddListener(AddItem);
    }

    public void ResetVisual()
    {
        foreach(var item in GameManager.instance.player.inventory.items.Keys)
        {
            AddItem(item);
        }
    }

    public void AddItem(SOItem toAdd)
    {
        if (!_itemHud.ContainsKey(toAdd))
        {
            ItemHudItem item = Instantiate(_itemUI, _layoutArea.transform).GetComponent<ItemHudItem>();
            _itemHud.Add(toAdd, item);
            item.SetImage(toAdd.icon);
            item.SetCount(GameManager.instance.player.inventory.items[toAdd]);
            item.transform.SetParent(_layoutArea.transform);
        }
        else
        {
            _itemHud[toAdd].SetCount(GameManager.instance.player.inventory.items[toAdd]);
        }

    }

}
