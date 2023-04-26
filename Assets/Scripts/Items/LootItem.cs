using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootItem : MonoBehaviour
{
    [SerializeField]
    private SOItem _item;
    public SOItem item
    {
        get => _item;
        set
        {
            _item = value;
            _itemImage.sprite = _item.icon;
        }
    }
    [SerializeField]
    private Image _itemImage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6) return;

        GameManager.instance.player.inventory.AddItem(_item);

        Destroy(gameObject);
    }
}
