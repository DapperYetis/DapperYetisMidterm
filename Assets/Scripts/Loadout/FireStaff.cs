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
    [SerializeField] AudioClip[] _primaryWeaponAudio;
    [SerializeField][Range(0f, 1f)] float _primaryWeaponAudioVol;
    [SerializeField] AudioClip[] _secondaryWeaponAudio;
    [SerializeField][Range(0f, 1f)] float _secondaryWeaponAudioVol;


    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Fire") && _canUsePrimary)
        {
            _flame.gameObject.SetActive(true);
            if(_primaryWeaponAudio.Length > 0)
            {
                _audio.clip = _primaryWeaponAudio[Random.Range(0, _primaryWeaponAudio.Length)];
                _audio.loop = true;
                _audio.Play();
            }
            _flame.SetStats(_stats.primaryAbility);
            _canUsePrimary = false;
        }
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            if (_secondaryWeaponAudio.Length > 0)
            {
                _audio.PlayOneShot(_secondaryWeaponAudio[Random.Range(0, _secondaryWeaponAudio.Length)], _secondaryWeaponAudioVol);
            }
            StartCoroutine(SecondaryFire());
        }
        if (Input.GetButtonUp("Primary Fire") && !_canUsePrimary)
        {
            _flame.gameObject.SetActive(false);
            _audio.loop = false;
            _canUsePrimary = true;
        }
    }
}
