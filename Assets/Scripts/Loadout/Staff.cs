using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Staff : Weapon
{
    [Header("-----Audio-----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip[] _primaryWeaponAudio;
    [SerializeField][Range(0f, 1f)] float _primaryWeaponAudioVol;
    [SerializeField] AudioClip[] _secondaryWeaponAudio;
    [SerializeField][Range(0f, 1f)] float _secondaryWeaponAudioVol;

    protected override void Update()
    {
        if (Input.GetButton("Primary Fire") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            if (_primaryWeaponAudio.Length > 0)
                _audio.PlayOneShot(_primaryWeaponAudio[Random.Range(0, _primaryWeaponAudio.Length)], _primaryWeaponAudioVol);
        }
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            if (_secondaryWeaponAudio.Length > 0)
                _audio.PlayOneShot(_secondaryWeaponAudio[Random.Range(0, _secondaryWeaponAudio.Length)], _secondaryWeaponAudioVol);
        }
        base.Update();
    }
}
