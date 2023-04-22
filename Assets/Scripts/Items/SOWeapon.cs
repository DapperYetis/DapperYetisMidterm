using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Stats/Weapon")]
public class SOWeapon : ScriptableObject
{
    [SerializeField]
    private string _weaponName;
    public string weaponName => _weaponName;
    [SerializeField]
    private AbilityStats _primaryAbility;
    public AbilityStats primaryAbility => _primaryAbility;
    [SerializeField]
    private AbilityStats _secondaryAbility;
    public AbilityStats secondaryAbility => _secondaryAbility;
    [SerializeField]
    private GameObject _prefab;
    public GameObject prefab => _prefab;
}
