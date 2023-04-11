using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamageable
{
    // Stats
    [SerializeField]
    private PlayerStats _stats;
    public PlayerStats stats => _stats;
    
    
    // ------References------
    [SerializeField]
    private PlayerMovement _movement;
    public PlayerMovement movement => _movement;
    [SerializeField]
    private Inventory _inventory;
    public Inventory inventory => _inventory;

    // Events
    [HideInInspector]
    public UnityEvent OnHealthChange;

    // Instance variables
    private float _healthCurrent;

    private void Start()
    {
        _movement.SetStats(_stats);

        _healthCurrent = _stats.healthMax;
    }

    public void Damage(float damage)
    {
        _healthCurrent -= damage;

        if(_healthCurrent <= 0)
        {
            // Lose condition
        }

        OnHealthChange?.Invoke();
    }

    public void Heal(float health)
    {
        _healthCurrent += health;

        if(_healthCurrent > _stats.healthMax)
        {
            _healthCurrent = _stats.healthMax;
        }

        OnHealthChange?.Invoke();
    }

    public float GetHealthMax() => _stats.healthMax;

    public float GetHealthCurrent() => _healthCurrent;
}
