using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

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
    //bool isDashing;
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
        if (!shieldEnabled)
        {
            shieldEnabled = true;
            shieldField.SetActive(true);
        }
        else if (shieldEnabled)
        {
            shieldEnabled = false;
            shieldField.SetActive(false);
        }

        yield return null;
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        //isDashing = true;
        rb.AddForce(transform.forward * dashingSpeed, ForceMode.Impulse);
        _damageTrigger.gameObject.SetActive(true);
        GameManager.instance.player.movement.enabled = false;
        yield return new WaitForSeconds(dashingCooldown);
        //isDashing = false;
        _damageTrigger.gameObject.SetActive(false);
        GameManager.instance.player.movement.enabled = true;
        yield return new WaitForSeconds(stats.useRateSecondary);
        _canUseSecondary = true;
    }

    protected void DamagedShield()
    {

    }
}
