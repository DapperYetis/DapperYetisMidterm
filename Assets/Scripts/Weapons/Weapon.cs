using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    [SerializeField] 
    protected Camera _camera;
    [SerializeField] 
    protected SOWeapon _stats;
    [SerializeField] 
    protected Transform _shootPos;

    protected bool _canUsePrimary = true;
    protected bool _canUseSecondary = true;

    // Update is called once per frame
    protected void Update()
    {
        if (Input.GetButton("Primary Fire") && _canUsePrimary && !GameManager.instance.isPaused)
            StartCoroutine(PrimaryFire());
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
            StartCoroutine(SecondaryFire());
    }

    protected IEnumerator PrimaryFire()
    {
        _canUsePrimary = false;

        Quaternion rot = _camera.transform.rotation;
        if(Physics.Raycast(_camera.ViewportPointToRay(new(0.5f, 0.5f)), out RaycastHit hit, _stats.primaryAbility.distance) && hit.transform.gameObject.layer == 7)
        {
            rot = Quaternion.LookRotation(hit.point - _shootPos.position);
        }
        Instantiate(_stats.primaryAbility.prefab, _shootPos.position, rot);
        yield return new WaitForSeconds(_stats.primaryAbility.cooldown);

        _canUsePrimary = true;
    }

    protected IEnumerator SecondaryFire()
    {
        _canUseSecondary = false;

        Quaternion rot = _camera.transform.rotation;
        if(Physics.Raycast(_camera.ViewportPointToRay(new(0.5f, 0.5f)), out RaycastHit hit, _stats.primaryAbility.distance) && hit.transform.gameObject.layer == 7)
        {
            rot = Quaternion.LookRotation(hit.point - _shootPos.position);
        }
        Instantiate(_stats.secondaryAbility.prefab, _shootPos.position, rot);
        yield return new WaitForSeconds(_stats.primaryAbility.cooldown);

        _canUseSecondary = true;
    }
}
