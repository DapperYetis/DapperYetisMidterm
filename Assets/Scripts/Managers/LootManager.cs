using ChanceSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    private static LootManager _instance;
    public static LootManager instance => _instance;

    [SerializeField]
    private GameObject _altarPrefab;
    private GameObject _altar;
    public GameObject altar => _altar;
    [SerializeField]
    private GameObject _chestPrefab;
    [SerializeField]
    private int _chestBaseCost = 30;
    [SerializeField]
    private float _chestCostRange = 0.5f;
    [SerializeField]
    private int _lootCount;
    [SerializeField]
    private Bounds _spawningBounds;
    private List<Transform> _lootLocations;

    [Header("---Items---")]
    [SerializeField]
    private List<SOItem> _items;
    private Dictionary<Rarity, List<SOItem>> _itemsByRarity;
    [SerializeField]
    private GameObject _noEffectPrefab;
    public GameObject noEffectPrefab => _noEffectPrefab;

    [SerializeField]
    private RandomItem<Rarity> _rarityChances;
    [SerializeField]
    private List<GameObject> _itemPrefabs;
    private Dictionary<Rarity, GameObject> _prefabByRarity;
    
    private void Start()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        
        ResetMap();
    }

    public void ResetMap()
    {
        SortPrefabs();
        SortItems();

        SetUpStage();
    }

    private void SortPrefabs()
    {
        _prefabByRarity = new(_itemPrefabs.Count);
        foreach(var prefab in _itemPrefabs)
        {
            _prefabByRarity[prefab.GetComponent<LootItem>().item.rarity] = prefab;
        }
    }

    private void SortItems()
    {
        _itemsByRarity = new(System.Enum.GetNames(typeof(Rarity)).Length)
        {
            [Rarity.Common] = (from item in _items where item.rarity == Rarity.Common select item).ToList(),
            [Rarity.Uncommon] = (from item in _items where item.rarity == Rarity.Uncommon select item).ToList(),
            [Rarity.Rare] = (from item in _items where item.rarity == Rarity.Rare select item).ToList(),
            [Rarity.Legendary] = (from item in _items where item.rarity == Rarity.Legendary select item).ToList(),
            [Rarity.Unique] = (from item in _items where item.rarity == Rarity.Unique select item).ToList()
        };
        _rarityChances.CalcWeight();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(_spawningBounds.center, _spawningBounds.size);
        if (!Application.isPlaying) return;

        Gizmos.color = Color.black;
        foreach(Transform trans in _lootLocations)
        {
            if(trans != null)
                Gizmos.DrawSphere(trans.position, 1);
        }
    }
#endif

    private void SetUpStage()
    {
        GenerateLoot();
    }

    private void GenerateLoot()
    {
        _lootLocations = new(_lootCount);
        Vector3 location = new();

        // Generate chest
        for(int i = 0; i < _lootCount; ++i)
        {
            GenerateLootLocation(ref location);

            if (Physics.Raycast(location, Vector3.down, out RaycastHit hitInfo))
            {
                location.y = hitInfo.point.y;
            }

            _lootLocations.Add(Instantiate(_chestPrefab, location, Quaternion.identity).transform);
            _lootLocations[^1].name = $"Loot item ({i + 1})";
        }

        // Generate Altar
        GenerateLootLocation(ref location);
        if (Physics.Raycast(location, Vector3.down, out RaycastHit hitInfo2))
        {
            location.y = hitInfo2.point.y;
        }

        _altar = Instantiate(_altarPrefab, location, Quaternion.identity);
    }

    private void GenerateLootLocation(ref Vector3 location)
    {
        location.x = Random.Range(_spawningBounds.min.x, _spawningBounds.max.x);
        location.y = _spawningBounds.center.y;
        location.z = Random.Range(_spawningBounds.min.z, _spawningBounds.max.z);
    }

    public SOItem GetItem()
    {
        Rarity rarity = _rarityChances.GetItem();
        //Debug.Log("RARITY: " + rarity);
        return _itemsByRarity[rarity][Random.Range(0, _itemsByRarity[rarity].Count)];
    }

    public GameObject GetPrefab(Rarity rarity)
    {
        if (_prefabByRarity.ContainsKey(rarity))
            return _prefabByRarity[rarity];

        Debug.LogError($"No loot prefab found for {rarity} rarity.");
        return null;
    }

    public int GetChestCost()
    {
        return (int)Random.Range(_chestBaseCost * EnemyManager.instance.scaleFactor, _chestBaseCost * Mathf.Ceil(EnemyManager.instance.scaleFactor + _chestCostRange));
    }
}
