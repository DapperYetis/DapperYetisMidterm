using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour, IDamageable, IBuffable
{
    // ------Stats------
    [SerializeField]
    private PlayerStats _stats;
    public PlayerStats stats => _stats;
    [SerializeField]
    private float _interactDistance;
    public float interactDistance => _interactDistance;
    public Dictionary<SOBuff, (int stacks, float time)> _currentBuffs = new();
    public Dictionary<SOBuff, BuffEffect> _currentBuffEffects = new();


    // ------References------
    [SerializeField]
    private Transform _effectsParent;
    [SerializeField]
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
    private PlayerController _instance;
    private SOWeapon _weaponAsset;
    private Weapon _weapon;
    public Weapon weapon => _weapon;
    
    private SOSupport _supportAsset;
    private Support _support;
    public Support support => _support;

    //private SOCompanion _companionAsset;
    //private Companion _companion;
    //public Companion companion => _companion;

    // ------Events------
    [HideInInspector]
    public UnityEvent<float> OnHealthChange;
    [HideInInspector]
    public UnityEvent OnPlayerSetUp;

    // Instance variables
    private float _healthCurrent;
    private bool _canInteract;
    private IInteractable _interactable;
    private bool _isSetUp;
    public bool isSetUp => _isSetUp;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void SetUp()
    {
        // Movement
        //_camera = Camera.main;
        _movement.SetStats(_stats);
        _movement.SetPosition(GameManager.instance.playerSpawnPos.position);

        // Combat
        GetLoadout();
        _weapon.SetCamera(_camera);
        _support.SetCamera(_camera);
        _inventory.OnItemsChange.AddListener(HandleNewItem);
        Heal(_stats.healthMax);
        OnPlayerSetUp.Invoke();

        _isSetUp = true;
    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.inGame) return;

        _interactable = null;
        UIManager.instance.PromptOff();
        _canInteract = false;

        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out RaycastHit hit, _interactDistance, 1 << 10))
        {
            _interactable = hit.transform.GetComponent<IInteractable>();
            if (_interactable != null)
            {
                UIManager.instance.PromptOn(_interactable.GetCost());
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
        CheckBuffs();
    }

    private void GetLoadout()
    {
        _weaponAsset = SettingsManager.instance.loadoutScript.GetWeapon();
        _weapon = Instantiate(_weaponAsset.prefab, transform).GetComponent<Weapon>();
        _weapon.SetStats(_weaponAsset.stats);

        _supportAsset = SettingsManager.instance.loadoutScript.GetSupport();
        _support = Instantiate(_supportAsset.prefab, transform).GetComponent<Support>();
        _support.SetStats(_supportAsset.stats);

        // TODO: Add companion setting once they are implemented
    }

    public void Damage(float damage, (SOBuff buff , int amount)[] buffs = null)
    {
        _healthCurrent -= damage;

        if (buffs != null)
        {
            foreach (var buff in buffs)
            {
                AddBuff(buff.buff, buff.amount);
            }
        }

        OnHealthChange?.Invoke(-damage);
    }

    public void Heal(float health)
    {
        _healthCurrent += health;

        if(_healthCurrent > _stats.healthMax)
        {
            _healthCurrent = _stats.healthMax;
        }

        OnHealthChange?.Invoke(health);
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

    public List<SOBuff> GetBuffs() => _currentBuffs.Keys.ToList();

    public void AddBuff(SOBuff buff, int amount = 1)
    {
        if (!_currentBuffs.ContainsKey(buff))
        {
            _currentBuffs.Add(buff, (0, Time.time + buff.buffLength));
            _currentBuffEffects.Add(buff, Instantiate(buff.effectPrefab, _effectsParent).GetComponent<BuffEffect>());
            _currentBuffEffects[buff].SetUp(this, buff);
        }
        _currentBuffs[buff] = (_currentBuffs[buff].stacks + amount, _currentBuffs[buff].time);
        _currentBuffEffects[buff].AddStacks(amount);
    }

    public void AddBuffs(List<(SOBuff buff, int count)> buffCounts)
    {
        foreach (var buff in buffCounts)
            AddBuff(buff.buff, buff.count);
    }

    public int GetStackCount(SOBuff buff)
    {
        return _currentBuffs.ContainsKey(buff) ? _currentBuffs[buff].stacks : 0;
    }

    public bool RemoveBuff(SOBuff buff)
    {
        if (!_currentBuffs.ContainsKey(buff)) return false;

        switch (buff.removeType)
        {
            case BuffRemoveType.Single:
                _currentBuffs[buff] = (_currentBuffs[buff].stacks - 1, Time.time + buff.buffLength);
                _currentBuffEffects[buff].RemoveStacks(1);
                break;
            case BuffRemoveType.Stack:
                _currentBuffs[buff] = (0, 0);
                _currentBuffEffects[buff].RemoveStacks(_currentBuffEffects[buff].stacks);
                break;
        }

        if (_currentBuffs[buff].stacks <= 0)
        {
            _currentBuffs.Remove(buff);
            _currentBuffEffects.Remove(buff);
            return true;
        }

        return false;
    }

    private void CheckBuffs()
    {
        for(int i = 0; i < _currentBuffs.Count; ++i)
        {
            if (Time.time < _currentBuffs[_currentBuffs.Keys.ElementAt(i)].time) continue;

            if (RemoveBuff(_currentBuffs.Keys.ElementAt(i)))
                --i;
        }
    }

    public void ResetLoadout()
    {
        if(!isSetUp)
            return;
        inventory.ResetPlayer();
        Destroy(_weapon.gameObject);
        _weapon = null;
        Destroy(_support.gameObject); 
        _support = null;
        _currentBuffs.Clear();

        _isSetUp = false;
    }
}