using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance => _instance;

    private PlayerController _player;
    public PlayerController player => _player;
    public PlayerMovement playerMovement => _player.movement;
    private Transform _playerSpawnPos;
    public Transform playerSpawnPos => _playerSpawnPos;

    public bool isPaused => UIManager.instance.isPaused;

    private bool _inGame;
    public bool inGame => _inGame;
    private float _startTime;
    public float runTime => Time.time - _startTime;
    public float runTimeMinutes => runTime * 0.0166f;

    private void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
            return;
        }

        _instance = this;

        StartCoroutine(FindPlayer());
    }

    private void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        SceneManager.sceneLoaded += DoResetMap;
    }

    private IEnumerator FindPlayer()
    {
        _player = null;
        while(true)
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            if (_player != null) break;

            yield return new WaitForEndOfFrame();
        }

        _player.OnHealthChange.AddListener(EndConditions);
        _playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;
        Debug.Log("Player found!");
    }


    #region Game Loop

    public void StartGame()
    {
        _inGame = true;
        _startTime = Time.time;
    }

    public void EndConditions()
    {
        if (!_inGame || UIManager.instance.activeMenu != null) return;

        if (_player.GetHealthCurrent() <= 0)
        {
            if (EnemyManager.instance.GetEnemyListSize() <= 0) return;
            UIManager.instance.NextMenu(UIManager.instance.references.loseMenu);
            UIManager.instance.PauseState();
        }
        //else if (EnemyManager.instance.GetEnemyListSize() <= 0)
        //{
        //    UIManager.instance.NextMenu(UIManager.instance.references.winMenu);
        //    UIManager.instance.PauseState();
        //}
    }

    public void ResetMap()
    {
        _inGame = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void DoResetMap(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return;

        StartCoroutine(FindPlayer());
        if (EnemyManager.instance != null)
            EnemyManager.instance.ResetMap();
        if (UIManager.instance != null)
            UIManager.instance.TransitionToMainMenu();
    }
    #endregion
}
