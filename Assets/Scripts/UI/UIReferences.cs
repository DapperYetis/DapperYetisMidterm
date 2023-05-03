using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIReferences : MonoBehaviour
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

    [SerializeField] Slider _mouseSensitivity;

    public Slider mouseSensitivity => _mouseSensitivity;

    [SerializeField] Toggle _camInvert;

    public Toggle camInvert => _camInvert;

    [SerializeField] Toggle _ctrlRun;

    public Toggle ctrlRun => _ctrlRun;

    [SerializeField] Toggle _toggleSprint;

    public Toggle toggleSprint => _toggleSprint;

    [SerializeField] GameObject _interactPrompt;

    public GameObject interactPrompt => _interactPrompt;

    [SerializeField] GameObject _transitionScreen;
    public GameObject transitionScreen => _transitionScreen;

    [SerializeField] Animator _animator;

    public Animator animator => _animator;

    [SerializeField] LoadoutScript _loadoutScript;
    
    public LoadoutScript loadoutScript => _loadoutScript;

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

    [SerializeField] GameObject _sprintCtrl;

    public GameObject sprintCtrl => _sprintCtrl;

    [SerializeField] GameObject _sprintShift;

    public GameObject sprintShift => _sprintShift;

    [SerializeField] GameObject _keyBindsMenu;

    public GameObject keyBindsMenu => _keyBindsMenu;

    [SerializeField] Image _dynamicHealth;

    public Image dynamicHealth => _dynamicHealth;

}
