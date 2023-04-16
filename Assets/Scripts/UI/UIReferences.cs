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

    public Image image => _hpBar;

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

}
