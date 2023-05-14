using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SettingsManager))]
public class LoadoutScript : MonoBehaviour
{
    [Header("----- Components -----")]
    [SerializeField] GameObject _buttonPrefab;
    [SerializeField]
    private TextMeshPro _weaponPrimaryDesc;
    [SerializeField] 
    private TextMeshPro _weaponSecondaryDesc;
    [SerializeField]
    private TextMeshPro _supportPrimaryDesc;
    [SerializeField]
    private TextMeshPro _supportSecondaryDesc;
    private GameObject _weaponGroup;
    private GameObject _supportGroup;
    private GameObject _companionGroup;
    [SerializeField] List<SOWeapon> _weapons;
    private static List<SOWeapon> _cachedWeapons;
    [SerializeField] List<SOSupport> _supports;
    private static List<SOSupport> _cachedSupports;

    private Dictionary<SOWeapon, Button> _weaponButtons = new();
    private Dictionary<SOSupport, Button> _supportButtons = new();

    public void SetUp()
    {
        CacheOptions();

        if (!PlayerPrefs.HasKey("WeaponChoice"))
            PlayerPrefs.SetInt("WeaponChoice", 0);
        if (!PlayerPrefs.HasKey("SupportChoice"))
            PlayerPrefs.SetInt("SupportChoice", 0);

        if (PlayerPrefs.GetInt("WeaponChoice") >= _cachedWeapons.Count)
            PlayerPrefs.SetInt("WeaponChoice", 0);
        if (PlayerPrefs.GetInt("SupportChoice") >= _cachedSupports.Count)
            PlayerPrefs.SetInt("SupportChoice", 0);

        AddOptions();
    }

    private void CacheOptions()
    {
        _cachedWeapons = new(_weapons);
        _cachedSupports = new(_supports);

        _weapons.Clear();
        _supports.Clear();
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
        if(!_weaponButtons.ContainsKey(newWeapon))
            _weaponButtons.Add(newWeapon, weapon);
    }

    private void SetWeapon(Button button)
    {
        PlayerPrefs.SetInt("WeaponChoice", _cachedWeapons.IndexOf((from weapon in _cachedWeapons where _weaponButtons[weapon] == button select weapon).First()));
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
        PlayerPrefs.SetInt("SupportChoice", _cachedSupports.IndexOf((from support in _cachedSupports where _supportButtons[support] == button select support).First()));
        Debug.Log(GetSupport());
    }

    public void SupportOn()
    {
        foreach (var button in _supportButtons.Values)
        {
            button.interactable = true;
        }
    }


    
    #endregion

    #region Getters
    public SOWeapon GetWeapon()
    {
        int choice = PlayerPrefs.GetInt("WeaponChoice");
        return _cachedWeapons[choice];

    }

    public SOSupport GetSupport() => _cachedSupports[PlayerPrefs.GetInt("SupportChoice")];

    #endregion

    public void SetWeaponGroup(GameObject row)
    {
        _weaponGroup = row;
    }

    public void SetSupportGroup(GameObject row)
    {
        _supportGroup = row;
    }

    public void AddOptions()
    {
        FindGroups();
        if (_weaponGroup != null)
        {
            foreach(var button in _weaponButtons.Values)
            {
                if(button)
                    Destroy(button.gameObject);
            }
            _weaponButtons.Clear();

            foreach (var weapon in _cachedWeapons)
            {
                AddWeapon(weapon);
            }
            _weaponButtons[GetWeapon()].interactable = false;
        }
        if (_supportGroup != null)
        {
            foreach (var button in _supportButtons.Values)
            {
                if (button)
                    Destroy(button.gameObject);
            }
            _supportButtons.Clear();

            foreach (var support in _cachedSupports)
            {
                AddSupport(support);
            }
            _supportButtons[GetSupport()].interactable = false;
        }
    }

    private void FindGroups()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("MenuCanvas");
        if (!obj || !obj.TryGetComponent<MainMenuRefs>(out MainMenuRefs menu))
            return;

        _weaponGroup = menu.weaponGroup;
        _supportGroup = menu.supportGroup;
    }
}
