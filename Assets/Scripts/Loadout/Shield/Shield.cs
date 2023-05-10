using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using static UnityEngine.Rendering.DebugUI;

public class Shield : Support
{
    public GameObject shieldField;
    public Transform spawnPoint;
    public float spawnMaxDistance = 7;
    public Vector3 spawnOffset = new(0, 1, 0);

    public bool shieldEnabled;

    [SerializeField] private ProjectileMelee _damageTrigger;
    [SerializeField] float dashingSpeed = 25f;
    [SerializeField] float dashingCooldown = 0.2f;
    private float _shieldDurability;
    public Material oldMaterial;
    public Material newMaterial;
    private Rigidbody rb;

    private void Start()
    {
        rb = GameManager.instance.player.movement.rb;
        _damageTrigger.gameObject.SetActive(false);
        _damageTrigger.SetStats(_stats);
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Support") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Primary());
            OnPrimary.Invoke();
        }
        else if (Input.GetButtonDown("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Secondary());
            OnSecondary.Invoke();
        }
    }

    protected override IEnumerator Primary()
    {
        _canUsePrimary = false;
        if (!shieldEnabled)
        {
            shieldEnabled = true;
            shieldField.SetActive(true);
        }
        else if (shieldEnabled)
        {
            shieldEnabled = false;
            shieldField.SetActive(false);
            yield return new WaitForSeconds(stats.useRatePrimary);
        }

        _canUsePrimary = true;
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        rb.AddForce(transform.forward * dashingSpeed, ForceMode.Impulse);
        _damageTrigger.gameObject.SetActive(true);
        GameManager.instance.player.movement.enabled = false;
        yield return new WaitForSeconds(dashingCooldown);
        _damageTrigger.gameObject.SetActive(false);
        GameManager.instance.player.movement.enabled = true;
        yield return new WaitForSeconds(stats.useRateSecondary);
        _canUseSecondary = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        MeshRenderer meshRenderer = shieldField.GetComponent<MeshRenderer>();
        if (shieldEnabled)
        {
            _shieldDurability--;
        }
        if(_shieldDurability < 0.3 * stats.useCountPrimary)
        {
            meshRenderer.material = newMaterial;
        }
        if(_shieldDurability <= 0)
        {
            shieldEnabled = false;
            shieldField.SetActive(false);
            StartCoroutine(Cooldown());
            _shieldDurability = stats.useCountPrimary;
            meshRenderer.material = oldMaterial;
        }
    }

    IEnumerator Cooldown()
    {
        _canUsePrimary = false;
        yield return new WaitForSeconds(stats.useRatePrimary);
        _canUsePrimary = true;

    }
}
