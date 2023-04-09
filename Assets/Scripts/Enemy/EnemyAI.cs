using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamageable
{
    [Header("--- Components ---")]
    [SerializeField] Renderer model;

    [Header("--- Enemy Stats ---")]
    [Range(50, 200)][SerializeField] float _HPMax;
    private float _HPCurrent;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(float amount)
    {
        _HPCurrent -= amount;
        StartCoroutine(flashColor(Color.red));

        if (_HPCurrent <= 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator flashColor(Color clr)
    {
        Color mainColor = model.material.color;
        model.material.color = clr;
        yield return new WaitForSeconds(0.1f);
        model.material.color = mainColor;
    }

    public void Heal(float health)
    {
        _HPCurrent += health;
        StartCoroutine(flashColor(Color.green));

        if (_HPCurrent >= _HPMax)
            _HPCurrent = _HPMax;
    }

    public float GetHealthMax()
    {
        return _HPMax;
    }

    public float GetHealthCurrent()
    {
        return _HPCurrent;
    }
}
