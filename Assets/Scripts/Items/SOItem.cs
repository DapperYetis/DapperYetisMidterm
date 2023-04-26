using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class SOItem : ScriptableObject
{
    [SerializeField]
    private Sprite _icon;
    public Sprite icon => _icon;
    [SerializeField]
    private string _description;
    public string description => _description;
    [SerializeField]
    private Rarity _rarity;
    public Rarity rarity => _rarity;
    [SerializeField, FormerlySerializedAs("_statsModifications")]
    private PlayerStats _movementStats;
    public PlayerStats statsModification => _movementStats;
    [SerializeField]
    private WeaponStats _attackStats;
    public WeaponStats attackStats => _attackStats;
}
