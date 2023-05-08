using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOverworld : MonoBehaviour, IInteractable
{
    [Header("--- Components ---")]
    [SerializeField]
    protected int _cost = 1;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    private GameObject _portalObject;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audPortalVol;
    [SerializeField]
    protected AudioClip[] _audPortal;

    private void Start()
    {
        _cost = (int)(_cost * EnemyManager.instance.scaleFactor);
    }

    public bool Interact()
    {
        _portalObject.SetActive(true);
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
        return true;
    }

    public bool CanInteract()
    {
        return true;
    }

    public int GetCost() => _cost;
}
