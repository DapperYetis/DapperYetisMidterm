using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("----- Settings -----")]
    [SerializeField] AudioMixer _masterMixer;
    UIReferences _references;
    [Header("----- Temporary -----")]
    [SerializeField]
    private bool _inGame;



    float _origTimeScale;
    public float origTimeScale => _origTimeScale;
    PlayerController _playerController;
    EnemyManager _enemyManager;
    Stack<GameObject> _menuStack;

    GameObject _activeMenu
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

    public bool isPaused => _activeMenu != null;

    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        instance = this;

        
        _origTimeScale = Time.timeScale;
        _menuStack = new Stack<GameObject>();
        SetUp();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (_activeMenu != null && ReferenceEquals(_activeMenu, _references.pauseMenu))
            {
                PrevMenu();
                ResumeState();
            }
            else if(_activeMenu == null)
            {
                PauseState();
                NextMenu(_references.pauseMenu);
            }
        }
    }

    private void SetUp()
    {
        StartCoroutine(RefindReferences(() =>
        {
            _playerController = GameManager.instance.player;
            _playerController.OnHealthChange.AddListener(UpdateHealth);
            _enemyManager = EnemyManager.instance;
            _enemyManager.OnEnemyCountChange.AddListener(WinCondition);

            if (!_inGame)
            {
                PauseState();
                ToFirstMenu(_references.mainMenu);
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                TransitionToGame();
            }

            return true;
        }));
    }

    private IEnumerator RefindReferences(Func<bool> callback)
    {
        _references = null;
        while(true)
        {
            _references = GameObject.FindGameObjectWithTag("UIReferences")?.GetComponent<UIReferences>();
            if (_references != null) break;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("UI References found");

        callback();
    }

    #region SliderFuncts
    public void SetVolume(float volume)
    {
        if (volume < 1)
        {
            volume = .001f;
        }

        RefreshSlider(volume);
        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
        _masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);
    }

    
    public void SetVolumeFromSlider()
    {
        SetVolume(_references.soundSlider.value);
    }

    public void RefreshSlider(float volume)
    {
        _references.soundSlider.value = volume;
    }
    #endregion

    #region Menu Buttons
    public void TransitionToLoadout()
    {
        NextMenu(_references.loadoutMenu);
    }

    public void TransitionToGame()
    {
        _references.hud.SetActive(true);
    }

    public void ToSettings()
    {
        _activeMenu.SetActive(false);
        NextMenu(_references.settingsMenu);
    }

    public void TransitionToMainMenu()
    {
        Time.timeScale = _origTimeScale;
        SetUp();
    }
    #endregion

    #region Pauses
    public void PauseState()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
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
        _activeMenu.SetActive(true);
    }

    public void ToFirstMenu(GameObject newMenu)
    {
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    } 
    #endregion

    // TODO: Move to GameManager
    #region GameLoop
    public void UpdateHealth()
    {
        if (_activeMenu != null) return;

        if (_playerController.GetHealthCurrent() > 0)
        {
            float currHealth = (float)_playerController.GetHealthCurrent() / (float)_playerController.GetHealthMax();
            _references.image.fillAmount = currHealth;
        }
        else
        {
            if (EnemyManager.instance.GetEnemyListSize() <= 0) return;
            NextMenu(_references.loseMenu);
            PauseState();
        }
    }

    public void WinCondition()
    {
        if (_activeMenu != null) return;

        _references.enemyCount.text = _enemyManager.GetEnemyListSize().ToString("F0");

        if (EnemyManager.instance.GetEnemyListSize() <= 0)
        {
            NextMenu(_references.winMenu);
            PauseState();
        }
    }
    #endregion


    #region HUD Functionality
    public void UpdateScore()
    {

    }

    public void TrackCurrency()
    {

    } 

    #endregion

}
