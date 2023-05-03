using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SOItem _item;
    private int _cost;

    private void Start()
    {
        _item = LootManager.instance.GetItem();
        _cost = LootManager.instance.GetChestCost();
        transform.position += GetComponent<Collider>().bounds.extents.y * Vector3.up;
    }

    public bool Interact()
    {
        if (!CanInteract()) return false;

        LootItem loot = Instantiate(LootManager.instance.GetPrefab(_item.rarity), transform.position, transform.rotation).GetComponent<LootItem>();
        loot.item = _item;
        GameManager.instance.player.inventory.Spend(_cost);

        Destroy(gameObject);

        return true;
    }

    public bool CanInteract() => GameManager.instance.player.inventory.currency > _cost;

    public int GetCost() => _cost;
}
