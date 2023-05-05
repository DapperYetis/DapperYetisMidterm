using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingVals : MonoBehaviour
{
    public static SettingVals _instance;

    [SerializeField] Toggle _toggleSprint;
    [SerializeField] Toggle _invertCam;
    [SerializeField] Slider _volume;
    [SerializeField] Slider _sensitivity;
    [SerializeField] AudioMixer _masterMixer;

    void Start()
    {
        if(_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;
    }

   
    void Update()
    {
        
    }

    public void SetVolume(float volume)
    {
        if (volume < 1)
        {
            volume = .001f;
        }

        RefreshVolume(volume);
        PlayerPrefs.SetFloat("SavedMasterVolume", volume);
        _masterMixer.SetFloat("MasterVolume", Mathf.Log10(volume / 100) * 20f);
    }


    public void SetVolumeFromSlider()
    {
        SetVolume(_volume.value);
    }

    public void RefreshVolume(float volume)
    {
        _volume.value = volume;
    }




    public void SetSensitivity(float sensitivity)
    {
        if (sensitivity < 1)
        {
            sensitivity = 1.5f;
        }

        RefreshSensitivity(sensitivity);
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }


    public void SetSensitivityFromSlider()
    {
        SetSensitivity(_sensitivity.value);
    }

    public void RefreshSensitivity(float sensitivity)
    {
        _sensitivity.value = sensitivity;
    }





    public void SetSprintHold()
    {
        if (_toggleSprint.isOn)
            PlayerPrefs.SetInt("HoldSprint", 1);

        else
            PlayerPrefs.SetInt("HoldSprint", 0);
    }

    public bool GetSprintToggle()
    {
        if (PlayerPrefs.GetInt("HoldSprint") == 1)
            return true;
        else
            return false;
    }



    public void SetInvertCam()
    {
        if (_invertCam.isOn)
            PlayerPrefs.SetInt("CamCtrl", 1);

        else
            PlayerPrefs.SetInt("CamCtrl", 0);
    }

    public bool GetInvertChoice()
    {
        if (PlayerPrefs.GetInt("CamCtrl") == 1)
            return true;
        else
            return false;
    }

}
