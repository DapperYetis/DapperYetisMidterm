using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage _instance;

    private void Update()
    {
        if (Input.GetKeyDown("0"))
            LoadScene(0);
        if (Input.GetKeyDown("1"))
            LoadScene(1); 
    }

    private void Start()
    {
        if(_instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        _instance = this;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }


}
