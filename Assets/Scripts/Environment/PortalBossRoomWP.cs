using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBossRoomWP : MonoBehaviour
{
    private static int _bossIndex = 0;
    public static int bossIndex => _bossIndex;
    private static int _totalBosses;
    public static int totalBosses => _totalBosses;

    [Header("--- Components ---")]
    [SerializeField]
    protected GameObject _portalEnterIdle;
    [SerializeField]
    protected GameObject _portalEnterClose;
    [SerializeField]
    protected GameObject _portalExitOpen;
    [SerializeField]
    protected GameObject _portalExitIdle;
    [SerializeField]
    protected SphereCollider _proclamationCollider;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    protected SOWave[] _bossWaves;
    [SerializeField]
    protected Transform _bossSpawnPoint;

    bool _bossHasSpawned;

    [Header("--- Audio Controls ---")]
    [Range(0, 1)]
    [SerializeField]
    protected float _audPortalOpenVol;
    [SerializeField]
    protected AudioClip[] _audPortalOpen;
    [Range(0, 1)]
    [SerializeField]
    protected float _audBossExclamationVol;
    [SerializeField]
    protected AudioClip[] _audBossExclamation;

    float _entryIdleDur;
    float _entryCloseDur;
    float _exitOpenDur;
    Portal _whereThePortalGoing;

    private void Start()
    {
        _whereThePortalGoing = _portalExitIdle.GetComponent<Portal>();
        _entryIdleDur = _portalEnterIdle.GetComponent<ParticleSystem>().main.duration;
        _entryCloseDur = _portalEnterClose.GetComponent<ParticleSystem>().main.duration;
        _exitOpenDur = _portalExitOpen.GetComponent<ParticleSystem>().main.duration;
        _totalBosses = _bossWaves.Length;
        StartCoroutine(PlayerEntry());
    }

    private void Update()
    {
        if (_bossHasSpawned && EnemyManager.instance.enemies.Count < 1 && !_portalExitOpen.activeInHierarchy && !_portalExitIdle.activeInHierarchy)
        {
            PlayerWon();
        }
    }

    private IEnumerator PlayerEntry()
    {
        UIManager.instance.SetObjectiveDescription("Defeat the boss.");
        if(_audPortalOpen.Length > 0)
            _aud.PlayOneShot(_audPortalOpen[Random.Range(0, _audPortalOpen.Length)], _audPortalOpenVol);
        //Debug.Log($"{name} played a sound");
        if (!EnemyManager.instance.inBossRoom)
            EnemyManager.instance.EnterBossRoom(null);
        yield return new WaitForSeconds(_entryIdleDur + 1);
        _portalEnterIdle.SetActive(false);
        _portalEnterClose.SetActive(true);
        yield return new WaitForSeconds(_entryCloseDur);
        _portalEnterClose.SetActive(false);
    }

    private void PlayerWon()
    {
        UIManager.instance.TurnOffBossHealthBar();
        UIManager.instance.SetObjectiveDescription("Head back through the portal you entered from.");
        if (_audPortalOpen.Length > 0)
            _aud.PlayOneShot(_audPortalOpen[Random.Range(0, _audPortalOpen.Length)], _audPortalOpenVol);
        //Debug.Log($"{name} played a sound");
        StartCoroutine(PortalOpening());
        RenderSettings.fog = false;
    }

    private IEnumerator PortalOpening()
    {
        _portalExitOpen.SetActive(true);
        yield return new WaitForSeconds(_exitOpenDur);
        _portalExitOpen.SetActive(false);
        _portalExitIdle.SetActive(true);
        _whereThePortalGoing.SetBuildIndex(++_bossIndex + 1);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _aud.PlayOneShot(_audBossExclamation[Random.Range(0, _audBossExclamation.Length)], _audBossExclamationVol);
            //Debug.Log($"{name} played a sound");
            _proclamationCollider.enabled = false;

            EnemyManager.SpawnEnemy(_bossWaves[_bossIndex], _bossSpawnPoint.position);
            _bossHasSpawned = true;
        }
    }
}
