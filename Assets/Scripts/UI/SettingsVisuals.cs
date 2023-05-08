using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsVisuals : MonoBehaviour
{
    [SerializeField] Toggle _toggleSprint;
    [SerializeField] Toggle _invertCam;
    [SerializeField] Slider _musicVolume;
    [SerializeField] Slider _sfxVolume;
    [SerializeField] Slider _sensitivity;
    private AudioMixer _masterMixer;

    private void OnEnable()
    {
        _toggleSprint.isOn = SettingsManager.instance.GetSprintToggle();
        _invertCam.isOn = SettingsManager.instance.GetInvertChoice();
        _musicVolume.value = SettingsManager.instance.GetMusicVolume();
        _sfxVolume.value = SettingsManager.instance.GetSFXVolume();
        _sensitivity.value = SettingsManager.instance.GetSensitivity();
    }
    public void SetMusicFromSlider()
    {
        SettingsManager.instance.SetMusicVolume(_musicVolume.value);
    }

    public void SetSFXFromSlider()
    {
        SettingsManager.instance.SetMusicVolume(_musicVolume.value);
    }

    public void SetSensitivityFromSlider()
    {
        SettingsManager.instance.SetSensitivity(_sensitivity.value);
    }

    public void SetSprintToggle()
    {
        SettingsManager.instance.SetSprintHold(_toggleSprint.isOn);
    }

    public void SetInvertCam()
    {
        SettingsManager.instance.SetInvertCam(_invertCam.isOn);
    }

}
