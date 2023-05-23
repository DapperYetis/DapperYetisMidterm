using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Teleport : Support
{
    //public Camera MainCamera;
    [SerializeField]
    private GameObject _teleportBeaconPrefab;
    private GameObject _currentBeacon;
    [SerializeField] private Transform _shotPoint;
    private Transform _player;
    [SerializeField] private float _throwForce;
    private bool _beaconPlaced = false;
    private RaycastHit _hit;
    [SerializeField] private GameObject _particleEffectsPrimary;
    [SerializeField] private GameObject _particleEffectsSecondary;
    [SerializeField] AudioSource _audio;
    [SerializeField] private AudioClip _audioClipQucikTele;
    [SerializeField][Range(0f, 1f)] float _audQuickTeleVol;
    [SerializeField] private AudioClip _audioClipBeacon;
    [SerializeField][Range(0f, 1f)] float _audBeaconVol;

    protected void Start()
    {
        _player = GameManager.instance.player.transform;
        _particleEffectsPrimary.SetActive(false);
        _particleEffectsSecondary.SetActive(false);
    }
    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Support") && _canUsePrimary && !GameManager.instance.isPaused)
        {
            StartCoroutine(Primary());
        }
        else if (Input.GetButtonDown("Secondary Support") && _canUseSecondary && !GameManager.instance.isPaused)
        {
            if (_audioClipBeacon != null)
            {
                _audio.PlayOneShot(_audioClipBeacon, _audBeaconVol);
            }
            StartCoroutine(Secondary());
        }
    }

    protected override IEnumerator Primary()
    {
        if (ObjectToTeleport())
        {
            _canUsePrimary = false;
            _particleEffectsPrimary.SetActive(true);
            TeleportToView();
            OnPrimary.Invoke();
            yield return new WaitForSeconds(_stats.useRatePrimary);
            _particleEffectsPrimary.SetActive(false);
            _canUsePrimary = true;
        }
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        if (!_beaconPlaced || _currentBeacon == null)
        {
            _currentBeacon = Instantiate(_teleportBeaconPrefab, _shotPoint.position, Quaternion.identity) as GameObject;
            _currentBeacon.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);
            _beaconPlaced = true;
           // yield return new WaitForSeconds(1);
        }
        else
        {
            if (_currentBeacon != null)
            {
                _particleEffectsSecondary.SetActive(true);
                float teleportOffset = GameManager.instance.player.GetComponent<CapsuleCollider>().height / 2;
                Vector3 beaconPosition = _currentBeacon.transform.position;
                Vector3 teleportLocation = new Vector3(beaconPosition.x, beaconPosition.y + teleportOffset, beaconPosition.z);
                _player.transform.position = teleportLocation;
                Destroy(_currentBeacon);
                _currentBeacon = null;
                _beaconPlaced = false;
                OnSecondary.Invoke();
                yield return new WaitForSeconds(_stats.useRateSecondary);
                _particleEffectsSecondary.SetActive(false);
            }
        }
        _canUseSecondary = true;
    }
    private bool ObjectToTeleport()
    {
        Vector3 origin = _player.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        return Physics.Raycast(origin, direction, out _hit, _stats.distancePrimary);
    }

    private void TeleportToView()
    {
        _player.transform.position = _hit.point + _hit.normal * 3f;
        if (_audioClipQucikTele != null)
        {
            _audio.PlayOneShot(_audioClipQucikTele, _audQuickTeleVol);
        }

    }
}
