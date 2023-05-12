using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuNav : MonoBehaviour
{
    [SerializeField] MainMenuRefs _menuRef;
    private Stack<GameObject> _menuStack = new();
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
        if (!_transitionPlaying)
        {
            NextMenu(_menuRef.loadoutMenu);
        }
    }

    public void ToSettings()
    {
        if(!_transitionPlaying)
            NextMenu(_menuRef.settingsMenu);
    }

    public void ToKeyBinds()
    {
        if (!_transitionPlaying)
            NextMenu(_menuRef.keyBindsMenu);
    }

    public void BackButton()
    {
        if(!_transitionPlaying)
            PrevMenu();
    }

    internal void ToCredits()
    {
        if (!_transitionPlaying)
            NextMenu(_menuRef.creditsScreen);
    }

    public void ButtonClick()
    {
        _menuRef.buttonClick.PlayOneShot(_menuRef.buttonClip);
    }

    public void StartedPlaying()
    {
        _transitionPlaying = true;
    }

    public void StoppedPlaying()
    {
        _transitionPlaying = false;
    }

    public void LoadoutTransition()
    {
        _menuRef.loadoutTransitions.SetTrigger("ButtonNext");
    }

    public void LoadoutBackAnim()
    {
        _menuRef.loadoutTransitions.SetTrigger("ButtonBack");
    }
    #endregion

}
