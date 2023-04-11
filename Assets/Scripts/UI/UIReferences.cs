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

}
