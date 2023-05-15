using UnityEngine;
using UnityEngine.UI;

public class EnemyBuffItem : MonoBehaviour
{
    private Image _icon;

    public void SetBuffUI(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}
