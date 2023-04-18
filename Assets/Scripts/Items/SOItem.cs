using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item")]
public class SOItem : ScriptableObject
{
    public string description;
    public GameObject prefab;
    public Rarity rarity;
    public PlayerStats statsModification;
}
