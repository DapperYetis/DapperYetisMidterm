using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffHudItem : MonoBehaviour
{
    [SerializeField] Image _debuffSprite;
    [SerializeField] int _stackCount;

    public void SetDebuffUI(Image ToAdd)
    {
        _debuffSprite = ToAdd;
    }

}
