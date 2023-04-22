using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class SOItem : ScriptableObject
{
    [SerializeField]
    private string _description;
    public string description => _description;
    [SerializeField]
    private Rarity _rarity;
    public Rarity rarity => _rarity;
    [SerializeField]
    private PlayerStats _statsModifications;
    public PlayerStats statsModification => _statsModifications;
}
