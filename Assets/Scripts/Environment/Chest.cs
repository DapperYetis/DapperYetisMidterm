using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{

    private void Start()
    {
        transform.position += GetComponent<Collider>().bounds.extents.y * Vector3.up;
    }

    public bool Interact()
    {
        SOItem item = LootManager.instance.GetItem();
        LootItem loot = Instantiate(item.prefab, transform.position, transform.rotation).GetComponent<LootItem>();
        loot.item = item;

        Destroy(gameObject);

        return true;
    }
}
