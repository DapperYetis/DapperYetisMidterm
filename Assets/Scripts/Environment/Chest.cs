using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField]
    private SOItem _item;
    private int _cost;
    [SerializeField]
    private MeshRenderer _oldModel;
    [SerializeField]
    private MeshFilter _oldMaterial;
    [SerializeField]
    private List<GameObject> _animals = new();

    private void Start()
    {
        SetModel();
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

    IEnumerator LookAtPlayer()
    {

        yield return new WaitForSeconds(Random.Range(5, 15));

    }

    public bool CanInteract() => GameManager.instance.player.inventory.currency > _cost;

    public int GetCost() => _cost;

    private void SetModel()
    {
        int index = Random.Range(0, _animals.Count - 1);
        GameObject animal = Instantiate(_animals[index], transform);
        animal.transform.Translate(new Vector3(0, -0.25f, 0));
        this.transform.localScale = new Vector3(2, 2, 2);
    }
}
