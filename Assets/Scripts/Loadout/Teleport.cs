using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

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
    [SerializeField] private GameObject _particleEffects;
    [SerializeField] AudioSource _audio;
    [SerializeField] private AudioClip _audioClipQucikTele;
    [SerializeField][Range(0f, 1f)] float _audQuickTeleVol;
    [SerializeField] private AudioClip _audioClipBeacon;
    [SerializeField][Range(0f, 1f)] float _audBeaconVol;

    // Start is called before the first frame update
    protected void Start()
    {
        _player = GameManager.instance.player.transform;
        _particleEffects.gameObject.SetActive(false);
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
            if (_audioClipBeacon != null)
            {
                _audio.PlayOneShot(_audioClipBeacon, _audBeaconVol);
            }
            StartCoroutine(Secondary());
            OnSecondary.Invoke();
        }
    }

    protected override IEnumerator Primary()
    {
        _canUsePrimary = false;
        if (ObjectToTeleport() != null)
        {
            _particleEffects.gameObject.SetActive(true);
            TeleportToView();
        }
        yield return new WaitForSeconds(_stats.useRatePrimary);
        _particleEffects.gameObject.SetActive(false);
        _canUsePrimary = true;
    }

    protected override IEnumerator Secondary()
    {
        _canUseSecondary = false;
        if (!_beaconPlaced)
        {
            _currentBeacon = Instantiate(_teleportBeaconPrefab, _shotPoint.position, Quaternion.identity) as GameObject;
            _currentBeacon.GetComponent<Rigidbody>().AddForce(_camera.transform.forward * _throwForce, ForceMode.Impulse);
            _beaconPlaced = true;
            yield return new WaitForSeconds(1);
        }
        else
        {
            _particleEffects.gameObject.SetActive(true);
            float teleportOffset = GameManager.instance.player.GetComponent<CapsuleCollider>().height / 2;
            Vector3 beaconPosition = _currentBeacon.transform.position;
            Vector3 teleportLocation = new Vector3(beaconPosition.x, beaconPosition.y + teleportOffset, beaconPosition.z);
            _player.transform.position = teleportLocation;
            Destroy(_currentBeacon);
            _currentBeacon = null;
            _beaconPlaced = false;
            yield return new WaitForSeconds(_stats.useRateSecondary);
            _particleEffects.gameObject.SetActive(false);
        }
        _canUseSecondary = true;
    }
    private GameObject ObjectToTeleport()
    {
        Vector3 origin = _player.transform.position;
        Vector3 direction = Camera.main.transform.forward;

        if (Physics.Raycast(origin, direction, out _hit, _stats.distancePrimary))
        {
            return _hit.collider.gameObject;
        }
        else
        {
            return null;
        }
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
