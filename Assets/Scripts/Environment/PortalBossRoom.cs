using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBossRoom : MonoBehaviour
{
    private static int _bossIndex = 0;
    public static int bossIndex => _bossIndex;

    [Header("--- Components ---")]
    [SerializeField]
    protected GameObject _portalEnterObject;
    [SerializeField]
    protected GameObject _portalExitObject;
    [SerializeField]
    protected SphereCollider _proclamationCollider;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    protected SOWave[] _bossWave;
    [SerializeField]
    protected Transform _bossSpawnPoint;

    bool _bossHasSpawned;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audPortalVol;
    [SerializeField]
    protected AudioClip[] _audPortal;
    [Range(0, 1)]
    [SerializeField]
    protected float _audBossExclamationVol;
    [SerializeField]
    protected AudioClip[] _audBossExclamation;

    private void Start()
    {
        StartCoroutine(PlayerEntry());
    }

    private void Update()
    {
        if (_bossHasSpawned && EnemyManager.instance.enemies.Count < 1 && !_portalExitObject.activeInHierarchy)
        {
            PlayerWon();
        }
    }

    private IEnumerator PlayerEntry()
    {
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
        Debug.Log($"{name} played a sound");
        if (!EnemyManager.instance.inBossRoom)
            EnemyManager.instance.EnterBossRoom(null);
        yield return new WaitForSeconds(3);
        _portalEnterObject.SetActive(false);
    }

    private void PlayerWon()
    {
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
        Debug.Log($"{name} played a sound");
        _portalExitObject.SetActive(true);
        RenderSettings.fog = false;
        ++_bossIndex;
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.gameObject.layer == 6)
        {
            _aud.PlayOneShot(_audBossExclamation[Random.Range(0, _audBossExclamation.Length)], _audBossExclamationVol);
            Debug.Log($"{name} played a sound");
            _proclamationCollider.enabled = false;

            EnemyManager.SpawnEnemy(_bossWave[_bossIndex], _bossSpawnPoint.position);
            _bossHasSpawned = true;
        }
    }
}
