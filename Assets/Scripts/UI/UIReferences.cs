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

    [SerializeField] Image _effectsBar;

    public Image effectsBar => _effectsBar;

    [SerializeField] TextMeshProUGUI _enemyCount;

    public TextMeshProUGUI enemyCount => _enemyCount;

    [SerializeField] TextMeshProUGUI _currency;

    public TextMeshProUGUI currency => _currency;

    [SerializeField] TextMeshProUGUI _score;

    public TextMeshProUGUI score => _score;

    [SerializeField] TextMeshProUGUI _maxHealth;

    public TextMeshProUGUI maxHealth => _maxHealth;

    [SerializeField] TextMeshProUGUI _remainingHealth;

    public TextMeshProUGUI remainingHealth => _remainingHealth;

    [SerializeField] GameObject _damageIndicator;

    public GameObject damageIndicator => _damageIndicator;

    [SerializeField] GameObject _tabMenu;

    public GameObject tabMenu => _tabMenu;

    [SerializeField] TextMeshProUGUI _interactPrompt;

    public TextMeshProUGUI interactPrompt => _interactPrompt;

    [SerializeField] GameObject _transitionScreen;

    public GameObject transitionScreen => _transitionScreen;

    [SerializeField] CanvasGroup _loadingScreenValues;

    public CanvasGroup loadingScreenValues => _loadingScreenValues;

    [SerializeField] Animator _itemAnimator;

    public Animator itemAnimator => _itemAnimator;

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

    [SerializeField] TextMeshProUGUI _winTime;

    public TextMeshProUGUI winTime => _winTime;

    [SerializeField] TextMeshProUGUI _winScore;

    public TextMeshProUGUI winScore => _winScore;

    [SerializeField] TextMeshProUGUI _winDistance;

    public TextMeshProUGUI winDistance => _winDistance;

    [SerializeField] TextMeshProUGUI _winJumps;

    public TextMeshProUGUI winJumps => _winJumps;

    [SerializeField] TextMeshProUGUI _winGold;

    public TextMeshProUGUI winGold => _winGold;

    [SerializeField] TextMeshProUGUI _winItems;

    public TextMeshProUGUI winItems => _winItems;

    [SerializeField] TextMeshProUGUI _winBuys;

    public TextMeshProUGUI winBuys => _winBuys;

    [SerializeField] TextMeshProUGUI _winDamage;

    public TextMeshProUGUI winDamage => _winDamage;

    [SerializeField] TextMeshProUGUI _winCrits;

    public TextMeshProUGUI winCrits => _winCrits;

    [SerializeField] TextMeshProUGUI _winBosses;

    public TextMeshProUGUI winBosses => _winBosses;

    [SerializeField] TextMeshProUGUI _winHealth;

    public TextMeshProUGUI winHealth => _winHealth;

    [SerializeField] TextMeshProUGUI _winHealed;

    public TextMeshProUGUI winHealed => _winHealed;

    [SerializeField] TextMeshProUGUI _loseScore;

    public TextMeshProUGUI loseScore => _loseScore;

    [SerializeField] TextMeshProUGUI _loseTime;

    public TextMeshProUGUI loseTime => _loseTime;

    [SerializeField] TextMeshProUGUI _loseDistance;

    public TextMeshProUGUI loseDistance => _loseDistance;

    [SerializeField] TextMeshProUGUI _loseJumps;

    public TextMeshProUGUI loseJumps => _loseJumps;

    [SerializeField] TextMeshProUGUI _loseGold;

    public TextMeshProUGUI loseGold => _loseGold;

    [SerializeField] TextMeshProUGUI _loseItems;

    public TextMeshProUGUI loseItems => _loseItems;

    [SerializeField] TextMeshProUGUI _loseBuys;

    public TextMeshProUGUI loseBuys => _loseBuys;

    [SerializeField] TextMeshProUGUI _loseDamage;

    public TextMeshProUGUI loseDamage => _loseDamage;

    [SerializeField] TextMeshProUGUI _loseCrits;

    public TextMeshProUGUI loseCrits => _loseCrits;

    [SerializeField] TextMeshProUGUI _loseBosses;

    public TextMeshProUGUI loseBosses => _loseBosses;

    [SerializeField] TextMeshProUGUI _loseHealth;

    public TextMeshProUGUI loseHealth => _loseHealth;

    [SerializeField] TextMeshProUGUI _loseHealed;

    public TextMeshProUGUI loseHealed => _loseHealed;

    [SerializeField] Image _dynamicHealth;

    public Image dynamicHealth => _dynamicHealth;

    [SerializeField] GameObject _itemNotif;

    public GameObject itemNotif => _itemNotif;

    [SerializeField] AudioSource _audioControl;

    public AudioSource audioControl => _audioControl;

    [SerializeField] AudioClip _buttonClip;

    public AudioClip buttonClip => _buttonClip;

    [SerializeField] AudioClip _pickUpClip;

    public AudioClip pickUpClip => _pickUpClip;

    [SerializeField] Button _pauseQuitButton;

    public Button pauseQuitButton => _pauseQuitButton;

    [SerializeField] Button _winQuitButton;

    public Button winQuitButton => _winQuitButton;

    [SerializeField] Button _loseQuitButton;

    public Button loseQuitButton => _loseQuitButton;

    [SerializeField] GameObject _bossHealthBar;

    public GameObject bossHealthBar => _bossHealthBar;

    [SerializeField] Image _bossHealth;

    public Image bossHealth => _bossHealth;

    [SerializeField] TextMeshProUGUI _objective;

    public TextMeshProUGUI objective => _objective;

}
