using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonFunctions : MonoBehaviour
{
    public void PlayGame()
    {
        UIManager.instance.PopStack();
        UIManager.instance.TransitionToGame();
        UIManager.instance.ResumeState();

    }

    public void ToLoadout()
    {
        UIManager.instance.PopStack();
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
}
