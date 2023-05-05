using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public LoadoutScript _loadout;

    private MainMenuRefs _menuCanvas;
    public static SceneManage _instance;


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown("0"))
            LoadScene(0);
        if (Input.GetKeyDown("1"))
            LoadScene(1);
        if (Input.GetKeyDown("2"))
            LoadScene(2);
    }
#endif

    private void Start()
    {
        if(_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        _instance = this;
        _menuCanvas = GameObject.FindGameObjectWithTag("MenuCanvas").GetComponent<MainMenuRefs>();
        _loadout = _menuCanvas.loadout;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }


}
