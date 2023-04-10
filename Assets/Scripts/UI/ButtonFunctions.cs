using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        UIManager._instance.PopStack();
        UIManager._instance.TransitionToGame();
        UIManager._instance.ResumeState();

    }

    public void ToLoadout()
    {
        UIManager._instance.PopStack();
        UIManager._instance.TransitionToLoadout();

    }

    public void ToSettings()
    {
        UIManager._instance.ToSettings();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        UIManager._instance.PopStack();
        UIManager._instance.TransitionToMainMenu();
    }

    public void BackButton()
    {

        UIManager._instance.PrevMenu();

    }

    public void resumeButton()
    {

        UIManager._instance.PrevMenu();
        UIManager._instance.ResumeState();

    }
}
