using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HudItems : MonoBehaviour
{
    [SerializeField] GameObject _layoutArea;
    [SerializeField] GameObject _itemUI;
    [SerializeField] Image _itemSprite;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemDescription;

    private Dictionary<SOItem, ItemHudItem> _itemHud = new();


    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.player.inventory.OnItemsChange.AddListener(AddItem);
    }

    public void ResetVisual()
    {
        if (GameManager.instance.player.inventory.items == null) return;

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
        StartCoroutine(UpdateNotification(toAdd));
    }

    IEnumerator UpdateNotification(SOItem itemToAdd)
    {
        UIManager.instance.PlayPickUp();
        UIManager.instance.references.itemNotif.SetActive(true);
        _itemSprite.sprite = itemToAdd.icon;
        _itemName.SetText(itemToAdd.itemName);
        _itemDescription.SetText(itemToAdd.description);
        UIManager.instance.PickupAnimation();
        yield return new WaitForSeconds(3f);
        UIManager.instance.references.itemNotif.SetActive(false);
    }

}
