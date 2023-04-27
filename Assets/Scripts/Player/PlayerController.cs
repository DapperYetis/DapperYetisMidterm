using System;
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
    [SerializeField]
    private PlayerMovementGrappling _grappling;
    public PlayerMovementGrappling grappling => _grappling;


    // ------Loadout------
    private SOWeapon _weaponAsset;
    private Weapon _weapon;
    public Weapon weapon => _weapon;
    
    private SOSupport _supportAsset;
    private Support _support;
    public Support support => _support;
    
    //private SOCompanion _companionAsset;
    //private Companion _companion;
    //public Companion companion => _companion;

    // Events
    [HideInInspector]
    public UnityEvent OnHealthChange;
    [HideInInspector]
    public UnityEvent OnPlayerSetUp;

    // Instance variables
    private float _healthCurrent;
    private bool _canInteract;
    private IInteractable _interactable;

    public void StartGame()
    {
        // Movement
        _camera = Camera.main;
        _movement.SetStats(_stats);

        // Combat
        GetLoadout();
        _weapon.SetCamera(_camera);
        _inventory.OnItemsChange.AddListener(HandleNewItem);
        Heal(_stats.healthMax);
        OnPlayerSetUp.Invoke();
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.inGame) return;

        _interactable = null;
        UIManager.instance.references.interactPrompt.SetActive(false);
        _canInteract = false;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, 1 << 10))
        {
            _interactable = hit.transform.GetComponent<IInteractable>();
            if (_interactable != null)
            {
                UIManager.instance.references.interactPrompt.SetActive(true);
                _canInteract = true;
            }
        }
    }

    private void Update()
    {
        if (!GameManager.instance.inGame) return;

        if (_canInteract && Input.GetButtonDown("Interact"))
        {
            _interactable?.Interact();
        }
    }

    private void GetLoadout()
    {
        _weaponAsset = UIManager.instance.references.loadoutScript.GetWeapon();
        _weapon = Instantiate(_weaponAsset.prefab, transform).GetComponent<Weapon>();
        _weapon.SetStats(_weaponAsset.stats);

        _supportAsset = UIManager.instance.references.loadoutScript.GetSupport();
        _support = Instantiate(_supportAsset.prefab, transform).GetComponent<Support>();
        _support.SetStats(_supportAsset.stats);

        // TODO: Add companion setting once they are implemented
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
        _weapon.SetStats(_weapon.stats + item.attackStats);
        _movement.SetStats(_stats);

        if(item.statsModification.healthMax != 0)
        {
            Heal(item.statsModification.healthMax);
        }
    }
}
