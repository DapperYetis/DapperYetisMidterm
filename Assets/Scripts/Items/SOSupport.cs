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
    private string _primaryDescription;
    public string primaryDescription => _primaryDescription;

    [SerializeField] private string _secondaryDescription;
    public string secondaryDescription => _secondaryDescription;

    [SerializeField]
    private GameObject _prefab;
    public GameObject prefab => _prefab;

    [SerializeField]
    private SupportStats _stats;
    public SupportStats stats => _stats;
}
