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

    [SerializeField] Slider _soundSlider;

    public Slider soundSlider => _soundSlider;

    [SerializeField] Slider _mouseSensitivity;

    public Slider mouseSensitivity => _mouseSensitivity;

    [SerializeField] Toggle _camInvert;

    public Toggle camInvert => _camInvert;

    [SerializeField] Toggle _toggleSprint;

    public Toggle toggleSprint => _toggleSprint;

    [SerializeField] GameObject _transitionScreen;
    public GameObject transitionScreen => _transitionScreen;

    [SerializeField] Animator _animator;

    public Animator animator => _animator;

    [SerializeField] LoadoutScript _loadoutScript;

    public LoadoutScript loadoutScript => _loadoutScript;

    [SerializeField] GameObject _keyBindsMenu;

    public GameObject keyBindsMenu => _keyBindsMenu;
}
