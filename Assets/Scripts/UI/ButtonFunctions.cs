using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{

    MenuNav nav;

    public void PlayGame()
    {
        SceneManage._instance.LoadScene(1);
    }

    public void ToLoadout()
    {
        
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
       
    }
}
