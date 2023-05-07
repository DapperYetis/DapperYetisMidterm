using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(GrappleSwing))]
public class Grapple : Support
{
    protected GrappleSwing _grappleSwing;
    public GrappleSwing grappleSwing => _grappleSwing;
    protected bool _isStopping = false;

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

        //else if (Input.GetButton("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
        //    StartCoroutine(Secondary());
    }

    private void StartPrimary()
    {
        _canUsePrimary = false;
        _grappleSwing.StartSwing();
    }

    private void StartSecondary()
    {
        _canUseSecondary = false;
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
        yield return new WaitForSeconds(_stats.useRateSecondary);
        _canUseSecondary = true;
    }
}
