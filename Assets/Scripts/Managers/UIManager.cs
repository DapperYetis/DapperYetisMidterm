using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;


    [Header("----- Settings -----")]
    [SerializeField] Slider _soundSlider;
    [SerializeField] AudioMixer _masterMixer;
    [SerializeField] UIReferences _references;
    [Header("----- Temporary -----")]
    [SerializeField]
    private bool _inGame;



    float _origTimeScale;
    PlayerController _playerController;
    EnemyManager _enemyManager;
    Stack<GameObject> _menuStack;

    GameObject _activeMenu
    {
        get
        {
            if (_menuStack != null)
            {
                return _menuStack.Peek();
            }
            else { return null; }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _instance = this;
        _playerController = GameManager.instance.player;
        _playerController.OnHealthChange.AddListener(UpdateHealth);
        _enemyManager = EnemyManager.instance;
        _enemyManager.OnEnemyCountChange.AddListener(WinCondition);
        _menuStack = new Stack<GameObject>();

        if(!_inGame)
        {
            _origTimeScale = Time.timeScale;
            PauseState();
            ToFirstMenu(_references.mainMenu);

        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (GameObject.ReferenceEquals(_activeMenu, _references.hud))
            {
                PauseState();
                NextMenu(_references.pauseMenu);
            }
            else
            {
                if (GameObject.ReferenceEquals(_activeMenu, _references.pauseMenu))
                {
                    PrevMenu();
                    ResumeState();
                }

            }
        }
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
        SetVolume(_soundSlider.value);
    }

    public void RefreshSlider(float volume)
    {
        _soundSlider.value = volume;
    } 
    #endregion

    public void TransitionToLoadout()
    {
        NextMenu(_references.loadoutMenu);
    }

    public void TransitionToGame()
    {
        ToFirstMenu(_references.hud);
    }

    public void ToSettings()
    {
        _activeMenu.SetActive(false);
        NextMenu(_references.settingsMenu);
    }

    public void PrevMenu()
    {
        PopStack();
        _activeMenu.SetActive(true);
    }

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

    public void TransitionToMainMenu()
    {
        _menuStack.Clear();
        ToFirstMenu(_references.mainMenu);
    }

    public void PopStack()
    {
        _activeMenu.SetActive(false);
        _menuStack.Pop();
    }

    public void NextMenu(GameObject newMenu)
    {
        _activeMenu.SetActive(false);
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    }

    public void ToFirstMenu(GameObject newMenu)
    {
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    }

    #region GameLoop
    public void UpdateHealth()
    {
        if (_playerController.GetHealthCurrent() > 0)
        {
            float currHealth = (float)_playerController.GetHealthCurrent() / (float)_playerController.GetHealthMax();
            _references.image.fillAmount = currHealth;
        }

        else
        {
            NextMenu(_references.loseMenu);
            PauseState();
        }
    }

    public void WinCondition()
    {

        _references.enemyCount.text = _enemyManager.GetEnemyListSize().ToString("F0");

        if (EnemyManager.instance.GetEnemyListSize() <= 0)
        {
            NextMenu(_references.winMenu);
            PauseState();
        }
    } 
    #endregion
}
