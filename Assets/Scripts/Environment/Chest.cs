using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SOItem _item;

    private void Start()
    {
        _item = LootManager.instance.GetItem();
        transform.position += GetComponent<Collider>().bounds.extents.y * Vector3.up;
    }

    public bool Interact()
    {
        LootItem loot = Instantiate(LootManager.instance.GetPrefab(_item.rarity), transform.position, transform.rotation).GetComponent<LootItem>();
        loot.item = _item;

        Destroy(gameObject);

        return true;
    }
}
