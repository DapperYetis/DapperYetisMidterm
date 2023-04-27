using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Support : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    protected Camera _camera;
    protected SupportStats _stats;
    public SupportStats stats => _stats;
    protected SupportStats _statMods;

    protected bool _canUsePrimary = true;
    protected bool _canUseSecondary = true;

    [HideInInspector]
    public UnityEvent OnPrimary;
    [HideInInspector]
    public UnityEvent OnSecondary;

    public void SetCamera(Camera camera)
    {
        _camera = camera;
    }

    public void SetStats(SupportStats stats)
    {
        _stats = stats;
    }

    protected virtual void Update()
    {
        if (Input.GetButton("Primary Support") && _canUsePrimary && !GameManager.instance.isPaused)
            StartCoroutine(Primary());
        else if (Input.GetButton("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
            StartCoroutine(Secondary());
    }

    protected abstract IEnumerator Primary();

    protected abstract IEnumerator Secondary();
}
