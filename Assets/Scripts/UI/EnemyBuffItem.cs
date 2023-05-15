using UnityEngine;
using UnityEngine.UI;

public class EnemyBuffItem : MonoBehaviour
{
    [SerializeField] Image _icon;

    public void SetBuffUI(Sprite sprite)
    {
        _icon.sprite = sprite;
    }
}
