using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsManager : MonoBehaviour
{
    
    public static SettingsManager instance;
    [SerializeField] 
    private LoadoutScript _loadoutScript;


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

    public void SetVolume(float volume)
    {
        if (volume < 1)
        {
            volume = .001f;
        }

        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
    }

    public float GetVolume()
    {
        return PlayerPrefs.GetFloat("SavedMasterVolume");
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
