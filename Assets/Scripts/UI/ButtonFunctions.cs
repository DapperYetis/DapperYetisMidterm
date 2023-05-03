using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        while (UIManager.instance.activeMenu != null)
        {
            UIManager.instance.PopStack();
        }
        UIManager.instance.TriggerTransition();
        UIManager.instance.TransitionToGame();
        UIManager.instance.ResumeState();

    }

    public void ToLoadout()
    {
        UIManager.instance.TransitionToLoadout();
    }

    public void ToSettings()
    {
        UIManager.instance.ToSettings();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        GameManager.instance.ResetMap();
    }

    public void BackButton()
    {

        UIManager.instance.PrevMenu();

    }

    public void ResumeButton()
    {

        UIManager.instance.PrevMenu();
        UIManager.instance.ResumeState();

    }

    public void SetVolume()
    {
        UIManager.instance.SetVolumeFromSlider();
    }

    public void SetSensitivity()
    {
        UIManager.instance.SetSensitivityFromSlider();
    }

    public void SetSprintButton()
    {
        UIManager.instance.SetCtrlSprint();
    }
    
    public void SetInvertedControls()
    {
        UIManager.instance.SetInvertCam();
    }

    public void SetToggleSprint()
    {
        UIManager.instance.SetSprintHold();
    }

    public void ToKeybindsMenu()
    {
        UIManager.instance.ToKeybinds();
    }
}
