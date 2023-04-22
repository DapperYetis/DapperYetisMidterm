using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Support", menuName = "Stats/Support Item")]
public class SOSupport : ScriptableObject
{
    [SerializeField]
    private string _supportName;
    public string supportName => _supportName;
}
