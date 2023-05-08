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

    [SerializeField] Animator _animator;

    public Animator animator => _animator;

    [SerializeField] GameObject _keyBindsMenu;

    public GameObject keyBindsMenu => _keyBindsMenu;

    [SerializeField] GameObject _weaponGroup;

    public GameObject weaponGroup => _weaponGroup;

    [SerializeField] GameObject _supportGroup;

    public GameObject supportGroup => _supportGroup;

    [SerializeField] GameObject _companionGroup;

    public GameObject companionGroup => _companionGroup;

    [SerializeField] GameObject _creditsScreen;

    public GameObject creditsScreen => _creditsScreen;

    [SerializeField] AudioSource _buttonClick;

    public AudioSource buttonClick => _buttonClick;

    [SerializeField] AudioClip _buttonClip;

    public AudioClip buttonClip => _buttonClip;

}
