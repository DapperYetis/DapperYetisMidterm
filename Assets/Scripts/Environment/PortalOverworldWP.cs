using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PortalOverworldWP : MonoBehaviour, IInteractable
{
    [Header("--- Components ---")]
    [SerializeField]
    protected int _cost = 1;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    private GameObject _portalExitOpen;
    [SerializeField]
    private GameObject _portalExitIdle;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audPortalOpenVol;
    [SerializeField]
    protected AudioClip[] _audPortalOpen;

    float _exitOpenDur;

    private void Start()
    {
        _exitOpenDur = _portalExitOpen.GetComponent<ParticleSystem>().main.duration;
        _cost = (int)(_cost * EnemyManager.instance.scaleFactor);
    }

    public bool Interact()
    {
        StartCoroutine(OpenPortal());
        return true;
    }

    AudioClip _fixed;

    public IEnumerator OpenPortal()
    {
        _portalExitOpen.SetActive(true);
        _aud.PlayOneShot(_audPortalOpen[Random.Range(0, _audPortalOpen.Length)], _audPortalOpenVol);
        yield return new WaitForSeconds(_exitOpenDur);
        _portalExitOpen.SetActive(false);
        _portalExitIdle.SetActive(true);
    }

    public bool CanInteract()
    {
        return true;
    }

    public int GetCost() => _cost;
}
