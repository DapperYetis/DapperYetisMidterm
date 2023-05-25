using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.Rendering.DebugUI;

public class Shield : Support
{
    public GameObject shieldField;
    public GameObject shieldFieldVisual;
    public Transform spawnPoint;
    public float spawnMaxDistance = 7;
    public Vector3 spawnOffset = new(0, 1, 0);

    public bool shieldEnabled;

    [SerializeField] private ProjectileMelee _damageTrigger;
    [SerializeField] private GameObject _particleEffect;
    [SerializeField] float dashingSpeed = 50f;
    [SerializeField] private float _shieldDurability;
    public Material oldMaterial;
    public Material newMaterial;
    private Rigidbody rb;

    [Header("---- Audio ----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] private AudioClip _shieldActiveAud;
    [SerializeField][Range(0f, 1f)] float _shieldActiveVol;
    [SerializeField] private AudioClip _shieldDeactiveAud;
    [SerializeField][Range(0f, 1f)] float _shieldDeactiveVol;
    [SerializeField] private AudioClip _shieldDamagedAud;
    [SerializeField][Range(0f, 1f)] float _shieldDamagedVol;
    [SerializeField] private AudioClip _shieldDashAud;
    [SerializeField][Range(0f, 1f)] float _shieldDashVol;

    private void Start()
    {
        rb = GameManager.instance.player.movement.rb;
        _damageTrigger.gameObject.SetActive(false);
        _damageTrigger.SetStats(_stats);
        _particleEffect.gameObject.SetActive(false);
        shieldField.gameObject.SetActive(false);
    }

    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Support") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Primary());
        }
        else if (Input.GetButtonDown("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Secondary());
        }
    }

    protected override IEnumerator Primary()
    {
        _canUsePrimary = false;
        if (!shieldEnabled)
        {
            shieldEnabled = true;
            shieldField.SetActive(true);
            if (_shieldActiveAud != null)
            {
                _audio.PlayOneShot(_shieldActiveAud, _shieldActiveVol);
            }
        }
        else if (shieldEnabled && _canUsePrimary == false)
        {
            shieldEnabled = false;
            shieldField.SetActive(false);
            if (_shieldDeactiveAud != null)
            {
                _audio.PlayOneShot(_shieldDeactiveAud, _shieldDeactiveVol);
            }
            yield return new WaitForSeconds(1);
        }

        _canUsePrimary = true;
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        rb.AddForce(transform.forward * dashingSpeed, ForceMode.Impulse);
        _damageTrigger.gameObject.SetActive(true);
        _particleEffect.gameObject.SetActive(true);
        GameManager.instance.player.movement.enabled = false;
        if (_shieldDashAud != null)
        {
            _audio.PlayOneShot(_shieldDashAud, _shieldDashVol);
        }
        yield return new WaitForSeconds(_stats.distanceSecondary);
        _particleEffect.gameObject.SetActive(false);
        _damageTrigger.gameObject.SetActive(false);
        GameManager.instance.player.movement.enabled = true;
        OnSecondary.Invoke();
        yield return new WaitForSeconds(stats.useRateSecondary);
        _canUseSecondary = true;
    }

    protected void OnTriggerEnter(Collider other)
    {
        MeshRenderer meshRenderer = shieldFieldVisual.GetComponent<MeshRenderer>();
        if (shieldEnabled)
        {
            _shieldDurability--;
        }
        if(_shieldDurability < 0.3 * stats.useCountPrimary)
        {
            meshRenderer.material = newMaterial;
        }
        if(_shieldDurability == 0.3 * stats.useCountPrimary)
        {
            if (_shieldDamagedAud != null)
            {
                _audio.PlayOneShot(_shieldDamagedAud, _shieldDamagedVol);
            }
        }
        if (_shieldDurability <= 0)
        {
            shieldEnabled = false;
            shieldField.SetActive(false);
            if (_shieldDeactiveAud != null)
            {
                _audio.PlayOneShot(_shieldDeactiveAud, _shieldDeactiveVol);
            }
            StartCoroutine(Cooldown());
            _shieldDurability = stats.useCountPrimary;
            meshRenderer.material = oldMaterial;
        }
    }

    IEnumerator Cooldown()
    {
        _canUsePrimary = false;
        OnPrimary.Invoke();
        yield return new WaitForSeconds(stats.useRatePrimary);
        _canUsePrimary = true;

    }
}
