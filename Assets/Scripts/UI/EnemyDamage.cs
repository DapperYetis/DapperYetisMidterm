using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _damageNumber;
    [SerializeField]
    private Animator _damageAnimation;
    [SerializeField]
    private float _animTime;

    public void SetDamage(float damage)
    {
        _damageNumber.SetText(damage.ToString("F0"));
    }

    public void PlayDamageAnimation()
    {
        StartCoroutine(AnimationStart());
    }

    IEnumerator AnimationStart()
    {
        _damageAnimation.SetTrigger("Damage");
        yield return new WaitForSeconds(_animTime);
        Destroy(gameObject);
    }

}
