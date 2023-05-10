using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuffHudItems : MonoBehaviour
{
    [SerializeField] Image _buffSprite;
    [SerializeField] TextMeshProUGUI _buffStack;
    private int total;

    public void SetBuffUI(Sprite buffSprite)
    {
        _buffSprite.sprite = buffSprite; 
    }

    public void SetBuffStack(int ToAdd)
    {
        total += ToAdd;
        _buffStack.SetText(total.ToString());
    }
}
