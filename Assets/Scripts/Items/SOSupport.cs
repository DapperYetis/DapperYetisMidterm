using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support", menuName = "Stats/Support Item")]
public class SOSupport : ScriptableObject
{
    [SerializeField]
    private string _supportName;
    public string supportName => _supportName;

    [SerializeField]
    private GameObject _prefab;
    public GameObject prefab => _prefab;

    [SerializeField]
    private SupportStats _stats;
    public SupportStats stats => _stats;
}
