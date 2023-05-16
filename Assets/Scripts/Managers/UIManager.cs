using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("----- Settings -----")]
    private UIReferences _references;
    private Inventory _playerInv;
    [SerializeField] 
    private Animator _transition;
    [SerializeField]
    private Gradient _healthGradient;
    [SerializeField]
    private int _tallyTime;
    [SerializeField]
    private AnimationCurve _lerpSpeed;
    [SerializeField]
    private float _animationLength;
    [SerializeField]
    private float _loadingLength;

    public UIReferences references => _references;



    private float _origTimeScale;
    public float origTimeScale => _origTimeScale;
    private PlayerController _playerController;
    private Stack<GameObject> _menuStack;
    private bool _isPlaying = false;
    private float _endtime;
    private float _lastCurrency = 0;
    private bool _isHealthUpdating;
    [SerializeField]
    private float _healthWaitTime = 1f;

    private GameObject _activeMenu
    {
        get
        {
            if (_menuStack.Count > 0)
            {
                return _menuStack.Peek();
            }
            else { return null; }
        }
    }
    public GameObject activeMenu => _activeMenu;

    public bool isPaused => _activeMenu != null;

    void Awake()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        instance = this;

        _origTimeScale = Time.timeScale;
        SetUp();
        
        if(_playerInv != null)
            _lastCurrency = _playerInv.currency;

        if (Application.platform == RuntimePlatform.WebGLPlayer)
            _references.quitButton.enabled = false;
    }

    void Update()
    {
        if (GameManager.instance.inGame)
            _references.timer.SetText($"{(int)GameManager.instance.runTimeMinutes} : {(GameManager.instance.runTime % 60).ToString("F1")}");
        if (GameManager.instance.inGame && !_isPlaying)
        {
            if (Input.GetButtonDown("Cancel") || Input.GetKeyDown(KeyCode.P))
            {
                if (_activeMenu != null && ReferenceEquals(_activeMenu, _references.pauseMenu))
                {
                    PrevMenu();
                    ResumeState();
                }
                else if (_activeMenu == null)
                {
                    PauseState();
                    NextMenu(_references.pauseMenu);
                }
            }

            if (_activeMenu == null)
            {
                if (Input.GetKeyDown("tab"))
                {

                    _references.tabMenu.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;

                }

                if (Input.GetKeyUp("tab"))
                {

                    _references.tabMenu.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;

                }
            }
        }
    }


    #region StartupFunctionality

    private void SetUp()
    {
        _menuStack = new Stack<GameObject>();
        StartCoroutine(RefindReferences(() =>
        {
            _playerController = GameManager.instance.player;
            _playerInv = _playerController.inventory;
            _transition = _references.itemAnimator;
            _playerController.OnHealthChange.AddListener(UpdateHealth);
            _playerController.OnPlayerSetUp.AddListener(() =>
            {
                _playerController.weapon.OnPrimary.AddListener(AttackCD1);
                _playerController.weapon.OnSecondary.AddListener(AttackCD2);
                _playerController.support.OnPrimary.AddListener(SupportCD1);
                _playerController.support.OnSecondary.AddListener(SupportCD2);
                _playerController.inventory.OnCurrencyChange.AddListener(TrackCurrency);
            });
            GameManager.instance.OnScoreChange.AddListener(UpdateScore);

            SetHealth();
            EnemyManager.instance.OnEnemyCountChange.AddListener(UpdateEnemyCount);

            if (!GameManager.instance.inGame)
            {
                PauseState();
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                _references.hud.SetActive(true);

                UpdateHealth(0);
                UpdateEnemyCount();
                UpdateScore(0);
                TrackCurrency(0);
                _references.hud.GetComponent<HudItems>().ResetVisual();
            }

            return true;
        }));
    }

    private IEnumerator RefindReferences(Func<bool> callback)
    {
        _references = null;
        while (true)
        {
            _references = GameObject.FindGameObjectWithTag("UIReferences")?.GetComponent<UIReferences>();
            if (_references != null) break;
            yield return new WaitForEndOfFrame();
        }
        if (GameManager.instance.inGame)
            _references.hud.SetActive(true);
        Debug.Log("UI References found");

        callback();
    }

    #endregion

    #region Menu Buttons

    public void TransitionToGame()
    {
        GameManager.instance.StartGame();
    }

    public void ToSettings()
    {
        NextMenu(_references.settingsMenu);
    }

    public void SceneReset()
    {
        StopAllCoroutines();
        Time.timeScale = _origTimeScale;
        SetUp();

    }

    public void PlayClick()
    {
        _references.audioControl.PlayOneShot(_references.buttonClip);
    }
    #endregion

    #region Pauses
    public void PauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void ResumeState()
    {
        Time.timeScale = _origTimeScale;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    #endregion

    #region Button Logic
    public void PrevMenu()
    {
        PopStack();
        if (_activeMenu != null)
            _activeMenu.SetActive(true);
    }

    public void PopStack()
    {
        _activeMenu.SetActive(false);
        _menuStack.Pop();
    }

    public void NextMenu(GameObject newMenu)
    {
        if (_activeMenu != null)
            _activeMenu.SetActive(false);
        _menuStack.Push(newMenu);
        if (_activeMenu != null)
            _activeMenu.SetActive(true);
    }

    public void StartsPlaying()
    {
        _isPlaying = true;
    }

    public void StopsPlaying()
    {
        _isPlaying = false;
        _references.transitionScreen.SetActive(false);
    }


    #endregion

    #region HUD Functionality
    public void UpdateHealth(float healthChange)
    {
        if (activeMenu != null || _references == null) return;

        if (_playerController.GetHealthCurrent() > 0)
        {
            if (_isHealthUpdating)
                _endtime = Time.time + _healthWaitTime;
            else
                StartCoroutine(DynamicHealthDecrease());
            if (healthChange < 0)
                StartCoroutine(Damaged());
            if (healthChange < 0)
                StartCoroutine(HealthRedFlash());
            SetHealth();
            float currHealth = (float)_playerController.GetHealthCurrent() / (float)_playerController.GetHealthMax();
            _references.hpBar.color = _healthGradient.Evaluate(currHealth);
            _references.hpBar.fillAmount = currHealth;
        }
    }

    public void UpdateStageCount(int stageIncrease)
    {
        references.stageCount.SetText(stageIncrease.ToString());
    }

    public void UpdateEnemyCount()
    {
        if (_activeMenu != null) return;

        references.enemyCount.text = EnemyManager.instance.GetEnemyListSize().ToString("F0");
    }
    public void UpdateScore(int newScore)
    {
        if (_references == null) return;
        _references.score.SetText(AchievementManager.instance.runStats.totalPoints.ToString());
    }

    public void TrackCurrency(int currency)
    {
        if (_references == null) return;
        StartCoroutine(LerpCurrency());
    }

    IEnumerator Damaged()
    {
        _references.damageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _references.damageIndicator.SetActive(false);
    }

    private void SetHealth()
    {
        if (_references == null) return;
        _references.maxHealth.SetText(_playerController.GetHealthMax().ToString());
        _references.remainingHealth.SetText(_playerController.GetHealthCurrent().ToString());
    }

    public void MaxHealthUpdate()
    {
        if (_references == null) return;
        _references.maxHealth.SetText(_playerController.GetHealthMax().ToString());
    }

    public void PromptOn(int cost)
    {
        if (_references == null) return;
        _references.interactPrompt.transform.parent.gameObject.SetActive(true);
        _references.interactPrompt.text = $"Press F To\nInteract\n${cost}";
    }

    public void PromptOff()
    {
        if (_references == null) return;
        _references.interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    public void AttackCD1()
    {
        if (_references == null) return;
        StartCoroutine(CooldownTimer(_playerController.weapon.stats.primaryAbility.cooldown, Time.time, _references.attCoolDwn1));
    }

    public void AttackCD2()
    {
        if (_references == null) return;
        StartCoroutine(CooldownTimer(_playerController.weapon.stats.secondaryAbility.cooldown, Time.time, _references.attCoolDwn2));
    }

    public void SupportCD1()
    {
        if (_references == null) return;
        StartCoroutine(CooldownTimer(_playerController.support.stats.useRatePrimary, Time.time, _references.suppCoolDwn1));
    }

    public void SupportCD2()
    {
        if (_references == null) return;
        StartCoroutine(CooldownTimer(_playerController.support.stats.useRateSecondary, Time.time, _references.suppCoolDwn2));
    }

    public void LoseScreenStats()
    {
        if (_references == null) return;
        _references.itemAnimator.enabled = false;
        _references.itemNotif.SetActive(false);
        _references.loseTime.SetText($"{(int)AchievementManager.instance.runStats.timePlayed} : {((AchievementManager.instance.runStats.timePlayed * 60) % 60).ToString("F1")}");
        StartCoroutine(SetScoreTally(_references.loseScore, (int)AchievementManager.instance.runStats.totalPoints));
        StartCoroutine(SetScoreTally(_references.loseDeaths, (int)(AchievementManager.instance.runStats.deaths)));
        StartCoroutine(SetScoreTally(_references.loseDistance, (int)(AchievementManager.instance.runStats.distanceMoved)));
        StartCoroutine(SetScoreTally(_references.loseJumps, (int)(AchievementManager.instance.runStats.jumps)));
        StartCoroutine(SetScoreTally(_references.loseGold, (int)(AchievementManager.instance.runStats.goldCollected)));
        StartCoroutine(SetScoreTally(_references.loseItems, (int)(AchievementManager.instance.runStats.itemsCollected)));
        StartCoroutine(SetScoreTally(_references.loseBuys, (int)(AchievementManager.instance.runStats.purchasesMade)));
        StartCoroutine(SetScoreTally(_references.loseDamage, (int)(AchievementManager.instance.runStats.damageDealt)));
        StartCoroutine(SetScoreTally(_references.loseCrits, (int)(AchievementManager.instance.runStats.criticalHits)));
        StartCoroutine(SetScoreTally(_references.loseBosses, (int)(AchievementManager.instance.runStats.bossesKilled)));
        StartCoroutine(SetScoreTally(_references.loseHealth, (int)(AchievementManager.instance.runStats.damageTaken)));
        StartCoroutine(SetScoreTally(_references.loseHealed, (int)(AchievementManager.instance.runStats.damageHealed)));
    }

    public void WinScreenStats()
    {
        if (_references == null) return;
        _references.itemAnimator.enabled = false;
        _references.itemNotif.SetActive(false);
        _references.winTime.SetText($"{(int)AchievementManager.instance.runStats.timePlayed} : {((AchievementManager.instance.runStats.timePlayed * 60) % 60).ToString("F1")}");
        StartCoroutine(SetScoreTally(_references.winScore, (int)(AchievementManager.instance.runStats.totalPoints)));
        StartCoroutine(SetScoreTally(_references.winDeaths, (int)(AchievementManager.instance.runStats.deaths)));
        StartCoroutine(SetScoreTally(_references.winDistance, (int)(AchievementManager.instance.runStats.distanceMoved)));
        StartCoroutine(SetScoreTally(_references.winJumps, (int)(AchievementManager.instance.runStats.jumps)));
        StartCoroutine(SetScoreTally(_references.winGold, (int)(AchievementManager.instance.runStats.goldCollected)));
        StartCoroutine(SetScoreTally(_references.winItems, (int)(AchievementManager.instance.runStats.itemsCollected)));
        StartCoroutine(SetScoreTally(_references.winBuys, (int)(AchievementManager.instance.runStats.purchasesMade)));
        StartCoroutine(SetScoreTally(_references.winDamage, (int)(AchievementManager.instance.runStats.damageDealt)));
        StartCoroutine(SetScoreTally(_references.winCrits, (int)(AchievementManager.instance.runStats.criticalHits)));
        StartCoroutine(SetScoreTally(_references.winBosses, (int)(AchievementManager.instance.runStats.bossesKilled)));
        StartCoroutine(SetScoreTally(_references.winHealth, (int)(AchievementManager.instance.runStats.damageTaken)));
        StartCoroutine(SetScoreTally(_references.winHealed, (int)(AchievementManager.instance.runStats.damageHealed)));
    }

    IEnumerator CooldownTimer(float cooldown, float startTime, TextMeshProUGUI target)
    {
        if (_references != null)
        {
            while ((cooldown - (Time.time - startTime) > 0))
            {
                target.SetText((cooldown - (Time.time - startTime)).ToString("F1"));
                yield return new WaitForSeconds(Time.deltaTime);
            }
            target.SetText("");
        }
        else
            yield return null;
    }

    IEnumerator DynamicHealthDecrease()
    {
        _isHealthUpdating = true;
        _endtime = Time.time + _healthWaitTime;
        while (Time.time < _endtime)
        {
            yield return new WaitForEndOfFrame();
        }
        _isHealthUpdating = false;
        StartCoroutine(LerpHealth());
    }

    IEnumerator HealthRedFlash()
    {
        _references.hpBar.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _references.hpBar.color = _healthGradient.Evaluate(_references.hpBar.fillAmount);
    }

    public void PlayPickUp()
    {
        _references.audioControl.PlayOneShot(_references.pickUpClip);
    }
    
    IEnumerator LerpCurrency()
    {
        float startTime = Time.time;
        while (Time.time < startTime + _animationLength)
        {
            _references.currency.SetText((Mathf.Lerp(_lastCurrency, _playerInv.currency, _lerpSpeed.Evaluate((Time.time - startTime) / _animationLength))).ToString("F0"));
            yield return new WaitForEndOfFrame();
        }
        _references.currency.SetText(_playerInv.currency.ToString());
        _lastCurrency = _playerInv.currency;
    }

    IEnumerator LerpHealth()
    {
        float startTime = Time.time;
        float healthStartAmount = _references.dynamicHealth.fillAmount;
        while (Time.time < startTime + _animationLength)
        {
            _references.dynamicHealth.fillAmount = Mathf.Lerp(healthStartAmount, _references.hpBar.fillAmount, _lerpSpeed.Evaluate((Time.time - startTime) / _animationLength));
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator SetScoreTally(TextMeshProUGUI toChange, int number)
    {
        float startTime = Time.realtimeSinceStartup;
        WaitForSecondsRealtime wait = new(0.02f);
        while(Time.realtimeSinceStartup < startTime + _tallyTime)
        {
            toChange.SetText((number * (Time.realtimeSinceStartup - startTime) / _tallyTime).ToString("F0"));
            yield return wait;
        }
        toChange.SetText(number.ToString());
    }

    public void PickupAnimation()
    {
        _transition.SetTrigger("Pickup");
    }

    public void StartLoading()
    {
        StartCoroutine(StartLoadingScreen());
    }

    IEnumerator StartLoadingScreen()
    {
        float startTime = Time.realtimeSinceStartup;
        _references.transitionScreen.SetActive(true);
        _references.loadingScreenValues.alpha = 0f;
        while(Time.realtimeSinceStartup < startTime + _loadingLength)
        {
            _references.loadingScreenValues.alpha = Mathf.Lerp(0, 1f, (Time.realtimeSinceStartup - startTime) / _loadingLength);
            yield return new WaitForEndOfFrame();
        }
        _references.loadingScreenValues.alpha = 1f;
    }

    public void StopLoading()
    {
        StartCoroutine(EndLoadingScreen());
    }

    IEnumerator EndLoadingScreen()
    {
        float startTime = Time.realtimeSinceStartup;
        _references.transitionScreen.SetActive(true);
        _references.loadingScreenValues.alpha = 1f;
        while (Time.realtimeSinceStartup < startTime + _loadingLength)
        {
            _references.loadingScreenValues.alpha = Mathf.Lerp(1f, 0, (Time.realtimeSinceStartup - startTime) / _loadingLength);
            yield return new WaitForEndOfFrame();
        }
        _references.transitionScreen.SetActive(false);
    }

    public void SetBossHealthbar(float maxhealth, float remainingHealth)
    {
        _references.bossHealth.fillAmount = remainingHealth / maxhealth;
    }

    public void TurnOnBossHealthBar()
    {
        references.bossHealthBar.SetActive(true);
    }

    public void TurnOffBossHealthBar()
    {
        references.bossHealthBar.SetActive(false);
    }

    #endregion
}
