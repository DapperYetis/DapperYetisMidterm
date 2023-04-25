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

    public void UpdateCount(int toAdd)
    {
        _total += toAdd;
       _count.SetText(_total.ToString());
    }

    public void SetImage(Image newImage)
    {
        _image = newImage;
    }
}
