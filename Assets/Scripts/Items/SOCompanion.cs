using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Companion", menuName = "Stats/Companion")]
public class SOCompanion : ScriptableObject
{
    [SerializeField]
    private string _companionName;
    public string companionName => _companionName;
}
