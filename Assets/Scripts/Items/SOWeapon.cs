using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Stats/Weapon")]
public class SOWeapon : ScriptableObject
{
    [SerializeField]
    private string _weaponName;
    [SerializeField]
    private string _primaryDesc;
    public string primaryDesc => _primaryDesc;
    [SerializeField]
    private string _secondaryDesc;
    public string secondaryDesc => _secondaryDesc;
    public string weaponName => _weaponName;
    [SerializeField]
    private GameObject _prefab;
    public GameObject prefab => _prefab;

    [SerializeField]
    private WeaponStats _stats;
    public WeaponStats stats => _stats;
}
