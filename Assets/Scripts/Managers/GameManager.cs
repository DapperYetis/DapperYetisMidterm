using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance => _instance;

    [SerializeField]
    private GameObject _playerPrefab;
    private PlayerController _startingPlayer;
    public PlayerController startingPlayer
    {
        get
        {
            if (_startingPlayer == null)
                _startingPlayer = _playerPrefab.GetComponent<PlayerController>();
            return _startingPlayer;
        }
    }
    private PlayerController _player;
    public PlayerController player => _player;
    public PlayerMovement playerMovement => _player.movement;
    private Transform _playerSpawnPos;
    public Transform playerSpawnPos => _playerSpawnPos;

    public bool isPaused => UIManager.instance.isPaused;

    private int buildIndex = 0;
    private bool _inGame => buildIndex > 0;
    public bool inGame => _inGame;
    private float _startTime;
    public float runTime => Time.time - _startTime;
    public float runTimeMinutes => runTime * 0.0166f;

    private int _score = 0;
    public int score => _score;
    [HideInInspector]
    public UnityEvent<int> OnScoreChange;

    private void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            Destroy(transform.parent.gameObject);
            return;
        }

        _instance = this;
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        FindPlayer();
    }

    private void Start()
    {
        DontDestroyOnLoad(transform.parent.gameObject);
        SceneManager.sceneLoaded += DoResetMap;

        if (inGame)
            _player.SetUp();
    }

    private void FindPlayer()
    {
        if (_player == null)
        {
            Instantiate(_playerPrefab);
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            _player.OnHealthChange.AddListener((amt) => EndConditions());
        }

        _playerSpawnPos = GameObject.FindGameObjectWithTag("PlayerSpawnPoint").transform;

        Debug.Log("Player found!");
    }


    #region Game Loop

    public void StartGame()
    {
        SceneManage.instance.LoadScene(1);
        _startTime = Time.time;
    }

    public void EndConditions()
    {
        if (!_inGame || UIManager.instance.activeMenu != null) return;

        if (_player.GetHealthCurrent() <= 0)
        {
            if (EnemyManager.instance.GetEnemyListSize() <= 0) return;
            UIManager.instance.LoseScreenStats(_score);
            UIManager.instance.NextMenu(UIManager.instance.references.loseMenu);
            UIManager.instance.PauseState();
        }
    }

    private void DoResetMap(Scene scene, LoadSceneMode mode)
    {
        buildIndex = scene.buildIndex;
        //if (mode == LoadSceneMode.Additive) return;

        FindPlayer();
        
        if (EnemyManager.instance != null)
            EnemyManager.instance.ResetMap();
        if (UIManager.instance != null)
            UIManager.instance.SceneReset();
        if (LootManager.instance != null)
            LootManager.instance.ResetMap();
        if (SettingsManager.instance != null)
            SettingsManager.instance.ResetMap();

        if (buildIndex == 0)
            player.ResetLoadout();
        if (buildIndex != 0 && !_player.isSetUp)
            _player.SetUp();
    }
    #endregion

    public void AddToScore(int addition)
    {
        _score += addition;
        OnScoreChange.Invoke(addition);
    }
}
