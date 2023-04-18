using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEditor.Experimental.GraphView.GraphView;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;


    [Header("----- Settings -----")]
    [SerializeField]
    private AudioMixer _masterMixer;
    private UIReferences _references;
    public UIReferences references => _references;
    [Header("----- Temporary -----")]
    [SerializeField]
    private bool _inGame;



    private float _origTimeScale;
    public float origTimeScale => _origTimeScale;
    private PlayerController _playerController;
    private Stack<GameObject> _menuStack;

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


    #region StartupFunctionality

    private void SetUp()
    {
        _menuStack = new Stack<GameObject>();
        StartCoroutine(RefindReferences(() =>
        {
            _playerController = GameManager.instance.player;
            _playerController.OnHealthChange.AddListener(UpdateHealth);
            SetHealth();
            EnemyManager.instance.OnEnemyCountChange.AddListener(UpdateEnemyCount);

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
        while (true)
        {
            _references = GameObject.FindGameObjectWithTag("UIReferences")?.GetComponent<UIReferences>();
            if (_references != null) break;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("UI References found");

        callback();
    }

    #endregion

    #region Settings
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

    public void SetSensitivity(float sensitivity)
    {
        if (sensitivity < 1)
        {
            sensitivity = 1.5f;
        }

        RefreshSensitivity(sensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }


    public void SetSensitivityFromSlider()
    {
        SetSensitivity(_references.mouseSensitivity.value);
    }

    public void RefreshSensitivity(float sensitivity)
    {
        _references.mouseSensitivity.value = sensitivity;
    }

    public void SetSprintHold()
    {
        if (_references.toggleSprint.isOn)
            PlayerPrefs.SetInt("HoldSprint", 1);

        else
            PlayerPrefs.SetInt("HoldSprint", 0);
    }

    public bool GetSprintToggle()
    {
        if (PlayerPrefs.GetInt("HoldSprint") == 1)
            return true;
        else
            return false;
    }

    public void SetCtrlSprint()
    {
        if (_references.ctrlRun)
            PlayerPrefs.SetInt("CrtlRun", 1);

        else if (!_references.ctrlRun)
            PlayerPrefs.SetInt("CtrlRun", 0);
    }

    public bool GetSprintKey()
    {
        if (PlayerPrefs.GetInt("CtrlRun") == 1)
            return true;
        else
            return false;
    }

    public void SetInvertCam()
    {
        if (_references.camInvert)
            PlayerPrefs.SetInt("CamCtrl", 1);

        else if (!_references.camInvert)
            PlayerPrefs.SetInt("CamCtrl", 0);
    }

    public bool GetInvertChoice()
    {
        if (PlayerPrefs.GetInt("CamCtrl") == 1)
            return true;
        else
            return false;
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
        GameManager.instance.StartGame();
    }

    public void ToSettings()
    {
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

    public void ToFirstMenu(GameObject newMenu)
    {
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    }
    #endregion

    #region HUD Functionality
    public void UpdateHealth()
    {
        if (UIManager.instance.activeMenu != null) return;

        if (_playerController.GetHealthCurrent() > 0)
        {
            StartCoroutine(Damaged());
            SetHealth();
            float currHealth = (float)_playerController.GetHealthCurrent() / (float)_playerController.GetHealthMax();
            _references.image.fillAmount = currHealth;
        }
    }

    public void UpdateEnemyCount()
    {
        if (_activeMenu != null) return;

        references.enemyCount.text = EnemyManager.instance.GetEnemyListSize().ToString("F0");
    }
    public void UpdateScore()
    {

    }

    public void TrackCurrency()
    {

    }

    IEnumerator Damaged()
    {
        _references.damageIndicator.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        _references.damageIndicator.SetActive(false);
    }

    private void SetHealth()
    {
        _references.maxHealth.SetText(_playerController.GetHealthMax().ToString());
        _references.remainingHealth.SetText(_playerController.GetHealthCurrent().ToString());
    }

    public void MaxHealthUpdate()
    {

    }

    public void PromptOn()
    {
        _references.interactPrompt.SetActive(true);
    }

    public void PromptOff()
    {
        _references.interactPrompt.SetActive(false);
    }

    #endregion

}
