using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetStats(WeaponStats stats)
    {
        _stats = stats;
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

        yield return Fire(_stats.primaryAbility);

        _canUsePrimary = true;
    }

    protected virtual IEnumerator SecondaryFire()
    {
        _canUseSecondary = false;
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

        Instantiate(ability.prefab, _shootPos.position, rot).GetComponent<Projectile>().SetStats(ability);

        yield return new WaitForSeconds(ability.cooldown);
    }
}
