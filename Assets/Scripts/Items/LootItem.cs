using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootItem : MonoBehaviour
{
    [SerializeField]
    private SOItem _item;
    public SOItem item
    {
        get => _item;
        set
        {
            _item = item;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != 6) return;

        GameManager.instance.player.inventory.AddItem(_item);

        Destroy(gameObject);
    }
}
