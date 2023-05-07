using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsVisuals : MonoBehaviour
{
    [SerializeField] Toggle _toggleSprint;
    [SerializeField] Toggle _invertCam;
    [SerializeField] Slider _volume;
    [SerializeField] Slider _sensitivity;
    private AudioMixer _masterMixer;

    private void OnEnable()
    {
        _toggleSprint.isOn = SettingsManager.instance.GetSprintToggle();
        _invertCam.isOn = SettingsManager.instance.GetInvertChoice();
        _volume.value = SettingsManager.instance.GetVolume();
        _sensitivity.value = SettingsManager.instance.GetSensitivity();
    }
    public void SetVolumeFromSlider()
    {
        SettingsManager.instance.SetVolume(_volume.value);
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
