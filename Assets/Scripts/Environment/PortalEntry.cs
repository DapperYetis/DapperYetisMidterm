using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalEntry : MonoBehaviour
{
    [SerializeField]
    protected Transform _spawnPoint;
    [SerializeField]
    protected AudioSource _aud;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audPortalVol;
    [SerializeField]
    protected AudioClip[] _audPortal;
    [Range(0, 1)]
    [SerializeField]
    protected float _audBossExclamationVol;
    [SerializeField]
    protected AudioClip[] _audBossExclamation;

    private void Start()
    {
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
    }

    private void PlayerEntry()
    {

    }

    private void OnTriggerExit(Collider other)
    {
        _aud.PlayOneShot(_audBossExclamation[Random.Range(0, _audBossExclamation.Length)], _audBossExclamationVol);
    }
}
