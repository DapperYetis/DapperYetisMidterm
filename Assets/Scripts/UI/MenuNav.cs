using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNav : MonoBehaviour
{
    [SerializeField] MainMenuRefs _menuRef;
    private Stack<GameObject> _menuStack = new();
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


    // Start is called before the first frame update
    void Start()
    {
        _menuStack.Push(_menuRef.mainMenu);
        _activeMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    }

    public void ToSettings()
    {

    }

    public void ToKeyBinds()
    {

    }



    #endregion

}
