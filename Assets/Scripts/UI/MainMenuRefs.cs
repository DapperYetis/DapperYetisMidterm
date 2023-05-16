using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuRefs : MonoBehaviour
{
    [Header("----- UI -----")]
    [SerializeField] GameObject _mainMenu;
    public GameObject mainMenu => _mainMenu;

    [SerializeField] GameObject _loadoutMenu;

    public GameObject loadoutMenu => _loadoutMenu;

    [SerializeField] GameObject _settingsMenu;

    public GameObject settingsMenu => _settingsMenu;

    [SerializeField] GameObject _transitionScreen;
    public GameObject transitionScreen => _transitionScreen;

    [SerializeField] CanvasGroup _fadeToGame;

    public CanvasGroup fadeToGame => _fadeToGame;

    [SerializeField] CanvasGroup _loadoutValues;

    public CanvasGroup loadoutValues => _loadoutValues;

    [SerializeField] CanvasGroup _settingsValues;

    public CanvasGroup settingsValues => _settingsValues;

    [SerializeField] CanvasGroup _creditsValues;

    public CanvasGroup creditsValues => _creditsValues;

    [SerializeField] CanvasGroup _controlsValues;

    public CanvasGroup controlsValues => _controlsValues;

    [SerializeField] GameObject _keyBindsMenu;

    public GameObject keyBindsMenu => _keyBindsMenu;

    [SerializeField] GameObject _weaponGroup;

    public GameObject weaponGroup => _weaponGroup;

    [SerializeField] GameObject _supportGroup;

    public GameObject supportGroup => _supportGroup;

    [SerializeField] GameObject _creditsScreen;

    public GameObject creditsScreen => _creditsScreen;

    [SerializeField] AudioSource _buttonClick;

    public AudioSource buttonClick => _buttonClick;

    [SerializeField] AudioClip _buttonClip;

    public AudioClip buttonClip => _buttonClip;

    [SerializeField] TextMeshProUGUI _weaponPrimary;

    public TextMeshProUGUI weaponPrimary => _weaponPrimary;

    [SerializeField] TextMeshProUGUI _weaponSecondary;

    public TextMeshProUGUI weaponSecondary => _weaponSecondary;

    [SerializeField] TextMeshProUGUI _supportPrimary;

    public TextMeshProUGUI supportPrimary => _supportPrimary;

    [SerializeField] TextMeshProUGUI _supportSecondary;

    public TextMeshProUGUI supportSecondary => _supportSecondary;

    [SerializeField] Button _quitButton;

    public Button quitButton => _quitButton;
}
