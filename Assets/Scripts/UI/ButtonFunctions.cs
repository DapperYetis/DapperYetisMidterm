using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    [SerializeField] MenuNav nav;

    public void PlayGame()
    {
        UIManager.instance.TransitionToGame();
    }

    public void ToLoadout()
    {
        nav.ToLoadoutMenu();
    }

    public void ToGameSettings()
    {
        UIManager.instance.ToSettings();
    }

    public void ToMenuSettings()
    {
        nav.ToSettings();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        UIManager.instance.PauseState();
        SceneManage._instance.LoadScene(0);
    }

    public void GameBackButton()
    {
        UIManager.instance.PrevMenu();
    }

    public void MainBackButton()
    {
        nav.BackButton();
    }

    public void ResumeButton()
    {
        UIManager.instance.PrevMenu();
        UIManager.instance.ResumeState();
    }

    public void SetVolume()
    {
        SettingVals._instance.SetVolumeFromSlider();
    }

    public void SetSensitivity()
    {
        SettingVals._instance.SetSensitivityFromSlider();
    }
    
    public void SetInvertedControls()
    {
        SettingVals._instance.SetInvertCam();
    }

    public void SetToggleSprint()
    {
        SettingVals._instance.SetSprintHold();
    }

    public void ToKeybindsMenu()
    {
        nav.ToKeyBinds();
    }
}
