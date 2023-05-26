using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [Header("-----Audio-----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip[] _primaryIceAud;
    [SerializeField][Range(0f, 1f)] float _primIceWeaponAudVol;
    [SerializeField] AudioClip[] _secIceWeaponAud;
    [SerializeField][Range(0f, 1f)] float _secIceWeaponAudVol;

    private void Start()
    {
        _audio.volume = 1f;
    }

    protected override void Update()
    {
        if (Input.GetButton("Primary Fire") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            if (_primaryIceAud.Length > 0)
            {
                //_audio.PlayOneShot(_primaryIceAud[Random.Range(0, _primaryIceAud.Length)], _primIceWeaponAudVol);
                _audio.clip = _primaryIceAud[Random.Range(0, _primaryIceAud.Length)];
                _audio.Play();
            }
        }
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            if (_secIceWeaponAud.Length > 0)
            {
                //_audio.PlayOneShot(_secIceWeaponAud[Random.Range(0, _secIceWeaponAud.Length)], _secIceWeaponAudVol);
                _audio.clip = _secIceWeaponAud[Random.Range(0, _secIceWeaponAud.Length)];
                _audio.Play();
            }
        }
        base.Update();
    }
}
