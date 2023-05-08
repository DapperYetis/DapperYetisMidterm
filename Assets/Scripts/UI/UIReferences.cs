using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReferences : MonoBehaviour
{
    [Header("----- UI -----")]
    
    [SerializeField] GameObject _settingsMenu;

    public GameObject settingsMenu => _settingsMenu;

    [SerializeField] GameObject _pauseMenu;

    public GameObject pauseMenu => _pauseMenu;
    
    [SerializeField] GameObject _hud;

    public GameObject hud => _hud;
    
    [SerializeField] GameObject _winMenu;

    public GameObject winMenu => _winMenu;
    
    [SerializeField] GameObject _loseMenu;

    public GameObject loseMenu => _loseMenu;
    
    [SerializeField] Image _hpBar;

    public Image hpBar => _hpBar;

    [SerializeField] Image _xpBar;

    public Image xpbar => _xpBar;

    [SerializeField] TextMeshProUGUI _enemyCount;

    public TextMeshProUGUI enemyCount => _enemyCount;

    [SerializeField] TextMeshProUGUI _currency;

    public TextMeshProUGUI currency => _currency;

    [SerializeField] TextMeshProUGUI _stageCount;

    public TextMeshProUGUI stageCount => _stageCount;

    [SerializeField] TextMeshProUGUI _score;

    public TextMeshProUGUI score => _score;

    [SerializeField] TextMeshProUGUI _maxHealth;

    public TextMeshProUGUI maxHealth => _maxHealth;

    [SerializeField] TextMeshProUGUI _remainingHealth;

    public TextMeshProUGUI remainingHealth => _remainingHealth;

    [SerializeField] TextMeshProUGUI _playerLevel;

    public TextMeshProUGUI playerLevel => _playerLevel;

    [SerializeField] GameObject _damageIndicator;

    public GameObject damageIndicator => _damageIndicator;

    [SerializeField] GameObject _tabMenu;

    public GameObject tabMenu => _tabMenu;

    [SerializeField] TextMeshProUGUI _interactPrompt;

    public TextMeshProUGUI interactPrompt => _interactPrompt;

    [SerializeField] GameObject _transitionScreen;

    public GameObject transitionScreen => _transitionScreen;

    [SerializeField] Animator _animator;

    public Animator animator => _animator;

    [SerializeField] TextMeshProUGUI _timer;

    public TextMeshProUGUI timer => _timer;

    [SerializeField] TextMeshProUGUI _attCoolDwn1;

    public TextMeshProUGUI attCoolDwn1 => _attCoolDwn1;

    [SerializeField] TextMeshProUGUI _attCoolDwn2;

    public TextMeshProUGUI attCoolDwn2 => _attCoolDwn2;

    [SerializeField] TextMeshProUGUI _suppCoolDwn1;

    public TextMeshProUGUI suppCoolDwn1 => _suppCoolDwn1;

    [SerializeField] TextMeshProUGUI _suppCoolDwn2;

    public TextMeshProUGUI suppCoolDwn2 => _suppCoolDwn2;

    [SerializeField] TextMeshProUGUI _companionCD;

    public TextMeshProUGUI companionCD => _companionCD;

    [SerializeField] TextMeshProUGUI _winTime;

    public TextMeshProUGUI winTime => _winTime;

    [SerializeField] TextMeshProUGUI _winScore;

    public TextMeshProUGUI winScore => _winScore;

    [SerializeField] TextMeshProUGUI _loseScore;

    public TextMeshProUGUI loseScore => _loseScore;

    [SerializeField] TextMeshProUGUI _loseTime;

    public TextMeshProUGUI loseTime => _loseTime;

    [SerializeField] Image _dynamicHealth;

    public Image dynamicHealth => _dynamicHealth;

    [SerializeField] GameObject _itemNotif;

    public GameObject itemNotif => _itemNotif;

    [SerializeField] AudioSource _buttonClick;

    public AudioSource buttonClick => _buttonClick;

    [SerializeField] AudioClip _buttonClip;

    public AudioClip buttonClip => _buttonClip;

}
