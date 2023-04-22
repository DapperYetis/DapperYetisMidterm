using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutScript : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject _buttonPrefab;
    [SerializeField] GameObject _weaponGroup;
    [SerializeField] GameObject _supportGroup;
    [SerializeField] GameObject _companionGroup;
    [SerializeField] List<SOWeapon> _weapons;
    [SerializeField] List<SOSupport> _support;
    [SerializeField] List<SOCompanion> _companions;

    private Dictionary<SOWeapon, Button> _weaponButtons = new();
    private Dictionary<SOSupport, Button> _supportButtons = new();
    private Dictionary<SOCompanion, Button> _companionButtons = new();

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("WeaponChoice"))
            PlayerPrefs.SetInt("WeaponChoice", 0);
        if (!PlayerPrefs.HasKey("SupportChoice"))
            PlayerPrefs.SetInt("SupportChoice", 0);
        if (!PlayerPrefs.HasKey("CompanionChoice"))
            PlayerPrefs.SetInt("CompanionChoice", 0);

        // TODO: add failsafe for out of bound indexes
        foreach(var weapon in _weapons)
        {
            AddWeapon(weapon);
        }
        _weaponButtons[GetWeapon()].interactable = false;

        foreach(var support in _support)
        {
            AddSupport(support);
        }
        _supportButtons[GetSupport()].interactable = false;

        foreach (var companion in _companions)
        {
            AddCompanion(companion);
        }
        _companionButtons[GetCompainion()].interactable = false;
    }

    #region Setters
    public void AddWeapon(SOWeapon newWeapon)
    {
        Button weapon = Instantiate(_buttonPrefab, _weaponGroup.transform).GetComponent<Button>();

        weapon.name = newWeapon.name;
        weapon.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = newWeapon.weaponName;

        weapon.onClick.AddListener(() =>
        {
            SetWeapon(weapon);
            WeaponOn();
            weapon.interactable = false;
        });
        _weaponButtons.Add(newWeapon, weapon);
    }

    private void SetWeapon(Button button)
    {
        PlayerPrefs.SetInt("WeaponChoice", _weapons.IndexOf((from weapon in _weapons where _weaponButtons[weapon] == button select weapon).First()));
        Debug.Log(GetWeapon());
    }

    public void WeaponOn()
    {
        foreach (var button in _weaponButtons.Values)
        {
            button.interactable = true;
        }
    }




    public void AddSupport(SOSupport newSupport)
    {
        Button support = Instantiate(_buttonPrefab, _supportGroup.transform).GetComponent<Button>();

        support.name = newSupport.name;
        support.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = newSupport.supportName;    

        support.onClick.AddListener(() =>
        {
            SetSupport(support);
            SupportOn();
            support.interactable = false;
        });
        _supportButtons.Add(newSupport, support);
    }

    private void SetSupport(Button button)
    {
        PlayerPrefs.SetInt("SupportChoice", _support.IndexOf((from support in _support where _supportButtons[support] == button select support).First()));
        Debug.Log(GetSupport());
    }

    public void SupportOn()
    {
        foreach (var button in _supportButtons.Values)
        {
            button.interactable = true;
        }
    }




    public void AddCompanion(SOCompanion newCompanion)
    {
        Button companion = Instantiate(_buttonPrefab, _companionGroup.transform).GetComponent<Button>();

        companion.name = newCompanion.name;
        companion.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = newCompanion.companionName;

        companion.onClick.AddListener(() =>
        {
            SetCompanion(companion);
            CompanionOn();
            companion.interactable = false;
        });
        _companionButtons.Add(newCompanion, companion);
    }

    private void SetCompanion(Button button)
    {
        PlayerPrefs.SetInt("CompanionChoice", _companions.IndexOf((from companion in _companions where _companionButtons[companion] == button select companion).First()));
        Debug.Log(GetCompainion());
    }

    private void CompanionOn()
    {
        foreach (var button in _companionButtons.Values)
        {
            button.interactable = true;
        }
    }
    #endregion

    #region Getters
    public SOWeapon GetWeapon() => _weapons[PlayerPrefs.GetInt("WeaponChoice")];

    public SOSupport GetSupport() => _support[PlayerPrefs.GetInt("SupportChoice")];

    public SOCompanion GetCompainion() => _companions[PlayerPrefs.GetInt("CompanionChoice")];

    #endregion


}
