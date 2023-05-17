using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class FireStaff : Weapon
{
    [SerializeField]
    protected ProjectileMelee _flame;

    [Header("-----Audio-----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip[] _primFireWeaponAud;
    [SerializeField][Range(0f, 1f)] float _primFireWeaponAudVol;
    [SerializeField] AudioClip[] _secFireWeaponAud;
    [SerializeField][Range(0f, 1f)] float _secFireWeaponAudVol;

    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Fire") && _canUsePrimary)
        {
            _flame.gameObject.SetActive(true);
            _audio.loop = false;
            if (_primFireWeaponAud.Length > 0)
            {
                _audio.clip = _primFireWeaponAud[Random.Range(0, _primFireWeaponAud.Length)];
                _audio.loop = true;
                _audio.Play();
            }
            _flame.SetStats(_stats.primaryAbility);
            _canUsePrimary = false;
        }
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            if (_secFireWeaponAud.Length > 0)
                _audio.PlayOneShot(_secFireWeaponAud[Random.Range(0, _secFireWeaponAud.Length)], _secFireWeaponAudVol);
            StartCoroutine(SecondaryFire());
        }
        if (Input.GetButtonUp("Primary Fire") && !_canUsePrimary)
        {
            _flame.gameObject.SetActive(false);
            _audio.loop = false;
            _canUsePrimary = true;
        }
        base.Update();
    }
}
