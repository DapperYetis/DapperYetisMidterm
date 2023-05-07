using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private float _minSaveInterval = 5f;
    private float _saveInterval => _minSaveInterval * 2;

    private static AchievementManager _instance;
    public static AchievementManager instance => _instance;

    private GameStats _runStats = new();
    public GameStats runStats => _runStats;

    private GameStats _gameStats = new();
    public GameStats gameStats => _gameStats;
    private static string _statsDestination;

    private Achievements _achievements = new();
    public Achievements achievements => _achievements;
    private static string _achievementsDestination;

    // Instance variables
    private bool _canSave = true;
    private bool _isPlayerMoving;

    private void Start()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
        _statsDestination = Application.persistentDataPath + "/stats.dat";
        _achievementsDestination = Application.persistentDataPath + "/achievements.dat";
        //Debug.Log("Save destination: " + Application.persistentDataPath);

        Load();

        SetUpEvents();
        InvokeRepeating(nameof(Save), _saveInterval, _saveInterval);
    }

    private void Load()
    {
        LoadStats();
        LoadAchievements();
        Debug.Log(_gameStats);
    }

    private void Update()
    {
        _gameStats.timePlayed += Time.deltaTime * 0.0166f;
        if (_isPlayerMoving)
        {
            _gameStats.distanceMoved += GameManager.instance.player.movement.playerVelocity.magnitude * Time.deltaTime;
        }
    }

    private void Save()
    {
        if (_canSave)
            StartCoroutine(DoSave());
    }

    private IEnumerator DoSave()
    {
        _canSave = false;
        SaveStats();
        SaveAchievements();
        yield return new WaitForSecondsRealtime(_minSaveInterval);
        _canSave = true;
    }

    private void LoadStats()
    {
        if (!File.Exists(_statsDestination))
        {
            SaveStats();
        }

        using (FileStream file = File.OpenRead(_statsDestination))
        {
            BinaryFormatter bf = new();
            _gameStats = (GameStats)bf.Deserialize(file);
        }
    }

    private void SaveStats()
    {
        using (FileStream file = File.Exists(_statsDestination) ? File.OpenWrite(_statsDestination) : File.Create(_statsDestination))
        {
            BinaryFormatter bf = new();
            bf.Serialize(file, _gameStats);
        }
    }

    private void LoadAchievements()
    {
        if (!File.Exists(_achievementsDestination))
        {
            SaveAchievements();
        }

        using (FileStream file = File.OpenRead(_achievementsDestination))
        {
            BinaryFormatter bf = new();
            _achievements = (Achievements)bf.Deserialize(file);
        }
    }

    private void SaveAchievements()
    {
        using (FileStream file = File.Exists(_achievementsDestination) ? File.OpenWrite(_achievementsDestination) : File.Create(_achievementsDestination))
        {
            BinaryFormatter bf = new();
            bf.Serialize(file, _achievements);
        }
    }

    private void SetUpEvents()
    {
        GameManager.instance.OnScoreChange.AddListener((points) =>
        {
            _runStats.totalPoints += (uint)points;
            _gameStats.totalPoints += (uint)points;

            Save();
        });

        GameManager.instance.player.OnMoveStart.AddListener(() =>
        {
            _isPlayerMoving = true;

            Save();
        });

        GameManager.instance.player.OnMoveStop.AddListener(() =>
        {
            _isPlayerMoving = false;

            Save();
        });

        GameManager.instance.player.OnJump.AddListener(() =>
        {
            ++_runStats.jumps;
            ++_gameStats.jumps;

            Save();
        });




        GameManager.instance.player.inventory.OnCurrencyChange.AddListener((amount) =>
        {
            if (amount > 0)
            {
                _runStats.goldCollected += (ulong)amount;
                _gameStats.goldCollected += (ulong)amount;
            }
            else
            {
                ++_runStats.purchasesMade;
                ++_gameStats.purchasesMade;
            }

            Save();
        });

        GameManager.instance.player.OnHit.AddListener((projectile, target) =>
        {
            _runStats.damageDealt += projectile.stats.directDamage;
            _gameStats.damageDealt += projectile.stats.directDamage;

            if(projectile.hasCrit)
            {
                ++_runStats.criticalHits;
                ++_gameStats.criticalHits;
            }

            Save();
        });

        // Add boss killed event here when next level portal is implemented




        GameManager.instance.player.OnTakeDamage.AddListener((damage) =>
        {
            _runStats.damageTaken += damage;
            _gameStats.damageTaken += damage;

            if (GameManager.instance.player.GetHealthCurrent() <= 0)
            {
                ++_runStats.deaths;
                ++_gameStats.deaths;
            }

            Save();
        });

        GameManager.instance.player.OnHeal.AddListener((health) =>
        {
            _runStats.damageHealed += health;
            _gameStats.damageHealed += health;

            Save();
        });
    }
}
