using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSettings : MonoBehaviour
{
    [SerializeField] Toggle _ctrlSprint;
    [SerializeField] Toggle _sprintHold;
    [SerializeField] Toggle _camInvert;


    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("CtrlRun"))
            _ctrlSprint.isOn = UIManager.instance.GetSprintKey();

        if (PlayerPrefs.HasKey("HoldSprint"))
            _sprintHold.isOn = UIManager.instance.GetSprintToggle();

        if (PlayerPrefs.HasKey("CamCtrl"))
            _camInvert.isOn = UIManager.instance.GetInvertChoice();

        if (PlayerPrefs.HasKey("SavedMasterVolume"))
            UIManager.instance.SetVolume(PlayerPrefs.GetFloat("SavedMasterVolume"));

        if (PlayerPrefs.HasKey("Sensitivity"))
            UIManager.instance.SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
            
    }
}
