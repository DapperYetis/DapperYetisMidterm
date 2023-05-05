using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSettings : MonoBehaviour
{
    [SerializeField] Slider _volume;
    [SerializeField] Slider _sensitivity;
    [SerializeField] Toggle _sprintHold;
    [SerializeField] Toggle _camInvert;


    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("HoldSprint"))
            _sprintHold.isOn = SettingVals._instance.GetSprintToggle();

        if (PlayerPrefs.HasKey("CamCtrl"))
            _camInvert.isOn = SettingVals._instance.GetInvertChoice();

        if (PlayerPrefs.HasKey("SavedMasterVolume"))
            _volume.value = PlayerPrefs.GetFloat("SavedMasterVolume");
        //SettingVals._instance.SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume"));

        if (PlayerPrefs.HasKey("Sensitivity"))
            _sensitivity.value = PlayerPrefs.GetFloat("Sensitivity");
                //SettingVals._instance.SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
            
    }
}
