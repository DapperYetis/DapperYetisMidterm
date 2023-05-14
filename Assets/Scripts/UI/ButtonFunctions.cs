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
        StartCoroutine(GameTransition());
    }

    public void ToLoadout()
    {
        nav.ButtonClick();
        nav.ToLoadoutMenu();
    }

    public void ToGameSettings()
    {
        UIManager.instance.PlayClick();
        UIManager.instance.ToSettings();
    }

    public void ToMenuSettings()
    {
        nav.ButtonClick();
        nav.ToSettings();
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToMainMenu()
    {
        StartCoroutine(TransitionToMainMenu());
    }

    IEnumerator TransitionToMainMenu()
    {
        UIManager.instance.PlayClick();
        yield return new WaitForSecondsRealtime(0.3f);
        UIManager.instance.PauseState();
        SceneManage.instance.LoadScene(0);
    }

    public void GameBackButton()
    {
        UIManager.instance.PlayClick();
        UIManager.instance.PrevMenu();
    }

    public void MainBackButton()
    {
        if (!nav.transitionPlaying)
        {
            nav.ButtonClick();
            nav.BackButton();
        }
    }

    public void ResumeButton()
    {
        UIManager.instance.PlayClick();
        UIManager.instance.PrevMenu();
        UIManager.instance.ResumeState();
    }

    public void ToKeybindsMenu()
    {
        nav.ButtonClick();
        nav.ToKeyBinds();
    }

    public void ToCreditsScreen()
    {
        nav.ButtonClick();
        nav.ToCredits();
    }

    IEnumerator GameTransition()
    {
        nav.ButtonClick();
        nav.BeginLoadScreen();
        yield return new WaitForSecondsRealtime(.5f);
        UIManager.instance.TransitionToGame();
    }
}
