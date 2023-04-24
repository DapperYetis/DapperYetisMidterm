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
    private GameObject _prefab;
    public GameObject prefab => _prefab;

    [SerializeField]
    private WeaponStats _stats;
    public WeaponStats stats => _stats;
}
