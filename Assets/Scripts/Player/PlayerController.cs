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
    [SerializeField]
    private float _interactDistance;
    public float interactDistance => _interactDistance;


    // ------References------
    private Camera _camera;
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
        _camera = Camera.main;
        _movement.SetStats(_stats);
        _inventory.OnItemsChange.AddListener(HandleNewItem);

        Heal(_stats.healthMax);
    }

    private void FixedUpdate()
    {
        if(Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, 1 << 10))
        {
            if(Input.GetButtonDown("Interact"))
            {
                IInteractable interactable = hit.transform.GetComponent<IInteractable>();

                interactable?.Interact();
            }
        }
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

    private void HandleNewItem(SOItem item)
    {
        _stats += item.statsModification;
        _movement.SetStats(_stats);
    }
}
