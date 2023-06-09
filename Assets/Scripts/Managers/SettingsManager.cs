using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{

    public static SettingsManager instance;
    [SerializeField]
    private LoadoutScript _loadoutScript;

    public UnityEvent _onMusicSliderChange;
    public UnityEvent _onSFXSliderChange;
    public UnityEvent _onSensitivityChange;

    public LoadoutScript loadoutScript => _loadoutScript;

    void Awake()
    {
        if (instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        instance = this;
        _loadoutScript.SetUp();
        DefaultSetUp();
    }

    private void Update()
    {
        if(!GameManager.instance.inGame)
        {
            _loadoutScript.SetWeaponDescriptions();
            _loadoutScript.SetSupportDescriptions();
        }
    }

    public void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("SavedMusicVolume", volume);
        _onMusicSliderChange.Invoke();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("SavedMusicVolume");
    }


    public void SetSFXVolume(float volume)
    {

        PlayerPrefs.SetFloat("SavedSFXVolume", volume);
        _onSFXSliderChange.Invoke();
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat("SavedSFXVolume");
    }


    public void SetSensitivity(float sensitivity)
    {

        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        _onSensitivityChange.Invoke();
    }

    public float GetSensitivity()
    {
        return PlayerPrefs.GetFloat("Sensitivity");
    }


    public void SetSprintHold(bool check)
    {
        PlayerPrefs.SetInt("HoldSprint", check ? 1 : 0);
    }

    public bool GetSprintToggle()
    {
        if (PlayerPrefs.GetInt("HoldSprint") == 1)
            return true;
        else
            return false;
    }

    public void SetInvertCam(bool check)
    {
        PlayerPrefs.SetInt("CamCtrl", check ? 1 : 0);
    }

    public bool GetInvertChoice()
    {
        if (PlayerPrefs.GetInt("CamCtrl") == 1)
            return true;
        else
            return false;
    }

    public void ResetMap()
    {
        _loadoutScript?.AddOptions();
    }

    public void DefaultSetUp()
    {
        if (!PlayerPrefs.HasKey("SavedMusicVolume"))
            PlayerPrefs.SetFloat("SavedMusicVolume", 0.5f);
        if (!PlayerPrefs.HasKey("Sensitivity"))
            PlayerPrefs.SetFloat("Sensitivity", 0.75f);
        if (!PlayerPrefs.HasKey("SavedSFXVolume"))
            PlayerPrefs.SetFloat("SavedSFXVolume", 0.5f);
    }
}
