using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI.Table;

public abstract class Weapon : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    protected Camera _camera;
    protected WeaponStats _stats;
    public WeaponStats stats => _stats;
    protected AbilityStats _primaryMods;
    protected AbilityStats _secondaryMods;

    [SerializeField] 
    protected Transform _shootPos;

    protected bool _canUsePrimary = true;
    protected bool _canUseSecondary = true;

    [HideInInspector]
    public UnityEvent OnPrimary;
    [HideInInspector]
    public UnityEvent OnSecondary;
    [HideInInspector]
    public UnityEvent<Projectile, IDamageable> OnHit;

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetStats(WeaponStats stats)
    {
        _stats = stats;
    }

    public void ApplyItem(SOItem item, SOWeapon weapon)
    {
        // Primary
        if(item.attackStats.primaryAbility.changeType == StatChangeType.Additive)
        {
            _stats.primaryAbility += item.attackStats.primaryAbility;
        }
        else
        {
            _stats.primaryAbility += weapon.stats.primaryAbility * item.attackStats.primaryAbility;
        }

        // Secondary
        if(item.attackStats.secondaryAbility.changeType == StatChangeType.Additive)
        {
            _stats.secondaryAbility += item.attackStats.secondaryAbility;
        }
        else
        {
            _stats.secondaryAbility += weapon.stats.secondaryAbility * item.attackStats.secondaryAbility;
        }
    }

    protected virtual void Update()
    {
        if (Input.GetButton("Primary Fire") && _canUsePrimary && !GameManager.instance.isPaused)
            StartCoroutine(PrimaryFire());
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
            StartCoroutine(SecondaryFire());
    }

    protected virtual IEnumerator PrimaryFire()
    {
        _canUsePrimary = false;
        OnPrimary?.Invoke();
        yield return Fire(_stats.primaryAbility);

        _canUsePrimary = true;
    }

    protected virtual IEnumerator SecondaryFire()
    {
        _canUseSecondary = false;
        OnSecondary?.Invoke();
        yield return Fire(_stats.secondaryAbility);

        _canUseSecondary = true;
    }

    protected virtual IEnumerator Fire(AbilityStats ability)
    {
        Quaternion rot = _camera.transform.rotation;
        if (Physics.Raycast(_camera.ViewportPointToRay(new(0.5f, 0.5f)), out RaycastHit hit,ability.lifetime) && hit.transform.gameObject.layer == 7)
        {
            rot = Quaternion.LookRotation(hit.point - _shootPos.position);
        }

        CreateProjectile(ability, rot);

        yield return new WaitForSeconds(ability.cooldown);
    }
    protected virtual void CreateProjectile(AbilityStats ability, Quaternion rot = default)
    {
        if (rot == default(Quaternion)) rot = Quaternion.identity;

        Projectile projectile = Instantiate(ability.prefab, _shootPos.position, rot).GetComponent<Projectile>();
        projectile.SetStats(ability);
        projectile.OnHit.AddListener((projectile, damageable) => OnHit.Invoke(projectile, damageable));
    }
}
