using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsLogic : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    [SerializeField] Camera _camera;
    [Range(1, 10)][SerializeField] int _shootDamage;
    [Range(0.1f, 5)][SerializeField] float _shootRate;
    [Range(1, 100)][SerializeField] int _shootDist;
    [SerializeField] Transform _shootPos;

    [SerializeField] GameObject _magicType;
    bool _isShooting;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Primary Fire") && !_isShooting && !GameManager.instance.isPaused)
            StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        _isShooting = true;

        Quaternion rot = _camera.transform.rotation;
        if(Physics.Raycast(_camera.ViewportPointToRay(new(0.5f, 0.5f)), out RaycastHit hit, _shootDist) && hit.transform.gameObject.layer == 7)
        {
            rot = Quaternion.LookRotation(hit.point - _shootPos.position);
        }
        Instantiate(_magicType, _shootPos.position, rot);
        yield return new WaitForSeconds(_shootRate);

        _isShooting = false;
    }
}
