using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHudItem : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] TextMeshProUGUI _count;

    int _total;

    public void SetCount(int total)
    {
        _total = total;
       _count.SetText(_total.ToString());
    }

    public void SetImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
