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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(SOItem toAdd)
    {
        if (!_itemHud.ContainsKey(toAdd))
        {
            ItemHudItem item = Instantiate(_itemUI, _layoutArea.transform).GetComponent<ItemHudItem>();
            _itemHud.Add(toAdd, item);
            item.SetImage(toAdd.icon);
            item.UpdateCount(1);
            item.transform.SetParent(_layoutArea.transform);
        }
        else
        {
            _itemHud[toAdd].UpdateCount(1);
        }

    }

}
