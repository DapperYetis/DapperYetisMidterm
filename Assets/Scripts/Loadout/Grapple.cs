using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(GrappleSwing))]
public class Grapple : Support
{
    protected GrappleSwing _grappleSwing;
    public GrappleSwing grappleSwing => _grappleSwing;
    [SerializeField]
    protected ZipToGrapple _zipToGrapple;
    public ZipToGrapple zipToGrapple => _zipToGrapple;
    protected bool _isStopping = false;

    [Header("-----Audio-----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip[] _primGrappleAud;
    [SerializeField][Range(0f, 1f)] float _primGrappleAudVol;
    [SerializeField] AudioClip[] _secGrappleAud;
    [SerializeField][Range(0f, 1f)] float _secGrappleAudVol;

    private void Start()
    {
        _grappleSwing = GetComponent<GrappleSwing>();
    }
    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Support") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            StartPrimary();
            OnPrimary.Invoke();
        }
        else if (Input.GetButtonUp("Primary Support") && !_canUsePrimary && !GameManager.instance.isPaused && !_isStopping)
        {
            StartCoroutine(Primary());
        }
        else if (Input.GetButton("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Secondary());
        }
    }

    private void StartPrimary()
    {
        _canUsePrimary = false;
        _grappleSwing.StartSwing();
        if (grappleSwing.pm.swinging == true)
        {
            if (_primGrappleAud != null)
            {
                _audio.PlayOneShot(_primGrappleAud[Random.Range(0, _primGrappleAud.Length)], _primGrappleAudVol);
            }
        }
    }

    protected override IEnumerator Primary()
    {
        _isStopping = true;
        _grappleSwing.StopSwing();
        yield return new WaitForSeconds(_stats.useRatePrimary);
        _canUsePrimary = true;
        _isStopping = false;
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        _zipToGrapple.ShootHook();
        if (_zipToGrapple.isGrappling == true)
        {
            if (_secGrappleAud != null)
            {
                _audio.PlayOneShot(_secGrappleAud[Random.Range(0, _secGrappleAud.Length)], _secGrappleAudVol);
            }
        }
        yield return new WaitForSeconds(_stats.useRateSecondary);
        _canUseSecondary = true;
    }
}
