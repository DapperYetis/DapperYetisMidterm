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
        if (sensitivity < 1)
        {
            sensitivity = 1.5f;
        }


        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
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
}
