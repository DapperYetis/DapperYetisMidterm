using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuNav : MonoBehaviour
{
    [SerializeField] MainMenuRefs _menuRef;
    private Stack<GameObject> _menuStack = new();
    [SerializeField]
    private float _animationTime;
    private bool _transitionPlaying = false;
    public bool transitionPlaying => _transitionPlaying;
    private GameObject _activeMenu
    {
        get
        {
            if (_menuStack.Count > 0)
            {
                return _menuStack.Peek();
            }
            else { return null; }
        }
    }
    public GameObject activeMenu => _activeMenu;

    void Start()
    {
        UIManager.instance.PauseState();
        ToFirstMenu(_menuRef.mainMenu);

        if(Application.platform == RuntimePlatform.WebGLPlayer)
            _menuRef.quitButton.enabled = false;

    }

    #region MenuNav

    public void PrevMenu()
    {
        PopStack();
        if (_activeMenu != null)
            _activeMenu.SetActive(true);
    }

    public void PopStack()
    {
        _activeMenu.SetActive(false);
        _menuStack.Pop();
    }

    public void NextMenu(GameObject newMenu)
    {
        if (_activeMenu != null)
            _activeMenu.SetActive(false);
        _menuStack.Push(newMenu);
        if (_activeMenu != null)
            _activeMenu.SetActive(true);
    }

    public void ToFirstMenu(GameObject newMenu)
    {
        _menuStack.Push(newMenu);
        _activeMenu.SetActive(true);
    }

    #endregion

    #region MenuButtons

    public void ToLoadoutMenu()
    {
        NextMenu(_menuRef.loadoutMenu);
        StartCoroutine(TransitionToMenu(_menuRef.loadoutValues));
    }

    public void ToSettings()
    {
        NextMenu(_menuRef.settingsMenu);
        StartCoroutine(TransitionToMenu(_menuRef.settingsValues));
    }

    public void ToKeyBinds()
    {
        NextMenu(_menuRef.keyBindsMenu);
        StartCoroutine(TransitionToMenu(_menuRef.controlsValues));
    }

    public void ToCredits()
    {
        NextMenu(_menuRef.creditsScreen);
        StartCoroutine(TransitionToMenu(_menuRef.creditsValues));
    }

    public void BackButton()
    {
        StartCoroutine(TransitionToPrevious(_activeMenu.GetComponent<CanvasGroup>()));
       
    }

    public void ButtonClick()
    {
        _menuRef.buttonClick.PlayOneShot(_menuRef.buttonClip);
    }

    public IEnumerator TransitionToMenu(CanvasGroup values)
    {
        values.interactable = false;
        float startTime = Time.realtimeSinceStartup;
        values.alpha = 0f;
        while (Time.realtimeSinceStartup < startTime + _animationTime)
        {
            values.alpha = Mathf.Lerp(0, 1f, (Time.realtimeSinceStartup - startTime) / _animationTime);
            yield return new WaitForSecondsRealtime(.0001f);
        }
        values.alpha = 1f;
        values.interactable = true;
    }

    public IEnumerator TransitionToPrevious(CanvasGroup values)
    {
        float startTime = Time.realtimeSinceStartup;
        values.alpha = 1f;
        while (Time.realtimeSinceStartup < startTime + _animationTime)
        {
            values.alpha = Mathf.Lerp(1f, 0f, (Time.realtimeSinceStartup - startTime) / _animationTime);
            yield return new WaitForSecondsRealtime(.0001f);
        }
        values.alpha = 0f;
        PrevMenu();
    }

    public void BeginLoadScreen()
    {
        StartCoroutine(LoadingStart());
    }

    IEnumerator LoadingStart()
    {
        ButtonClick();
        float startTime = Time.realtimeSinceStartup;
        _menuRef.transitionScreen.SetActive(true);
        _menuRef.fadeToGame.alpha = 0;
        while (Time.realtimeSinceStartup < startTime + _animationTime)
        {
            _menuRef.fadeToGame.alpha = Mathf.Lerp(0f, 1f, (Time.realtimeSinceStartup - startTime) / _animationTime);
            yield return new WaitForSecondsRealtime(0.0001f);
        }
        _menuRef.fadeToGame.alpha = 1f;

    }
    #endregion

}
