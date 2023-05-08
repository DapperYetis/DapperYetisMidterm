using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalBossRoom : MonoBehaviour
{
    [Header("--- Components ---")]
    [SerializeField]
    protected GameObject _portalEnterObject;
    [SerializeField]
    protected GameObject _portalExitObject;
    [SerializeField]
    protected BoxCollider _proclamationCollider;
    [SerializeField]
    protected AudioSource _aud;
    [SerializeField]
    protected string _sceneNextName;
    [SerializeField]
    protected SOWave _bossWave;
    [SerializeField]
    protected Transform _bossSpawnPoint;

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
        if (EnemyManager.instance.enemies.Count == 0)
        {
            StartCoroutine(PlayerWon());
        }
    }

    private IEnumerator PlayerEntry()
    {
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
        yield return new WaitForSeconds(3);
        _portalEnterObject.SetActive(false);
    }

    private IEnumerator PlayerWon()
    {
        yield return new WaitForSeconds(3);
        _aud.PlayOneShot(_audPortal[Random.Range(0, _audPortal.Length)], _audPortalVol);
        _portalExitObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(_sceneNextName);
    }

    private void OnTriggerExit(Collider other)
    {
        _aud.PlayOneShot(_audBossExclamation[Random.Range(0, _audBossExclamation.Length)], _audBossExclamationVol);
        _proclamationCollider.enabled = false;

        EnemyManager.SpawnEnemy(_bossWave, _bossSpawnPoint.position);
    }
}
