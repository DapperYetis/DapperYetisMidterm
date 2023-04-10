using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;


    [Header("----- Settings -----")]
    [SerializeField] Slider _soundSlider;
    [SerializeField] AudioMixer _masterMixer;
    [SerializeField] UIReferences _references;



    float _origTimeScale;
    PlayerController _playerController;
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
        PauseState();
        _menuStack = new Stack<GameObject>();
        _menuStack.Push(_references.mainMenu);

        _origTimeScale = Time.timeScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

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

    public void PrevMenu()
    {

        _activeMenu.SetActive(true);

    }

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

    public void TransitionToMainMenu()
    {
        _activeMenu.SetActive(false);
        _menuStack.Clear();
        NextMenu(_references.mainMenu);


    }

    public void PopStack()
    {
        _activeMenu.SetActive(false);
        _menuStack.Pop();
    }

   

    public void AddToStack()
    {

    }

    public void ToSettings()
    {
        _activeMenu.SetActive(false);
        NextMenu(_references.settingsMenu);

    }

    public void TransitionToLoadout()
    {
        NextMenu(_references.loadoutMenu);
    }

    public void TransitionToGame()
    {

        NextMenu(_references.hud);

    }

    public void ToMainMenu()
    {
        NextMenu(_references.mainMenu);
    }

    public void NextMenu(GameObject newMenu)
    {
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    }

    public void AddToStack(GameObject prevMenu)
    {



    }


}
