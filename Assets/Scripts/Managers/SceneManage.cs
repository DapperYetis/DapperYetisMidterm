using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;


#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown("0"))
            LoadScene(0);
        else if (Input.GetKeyDown("1"))
            LoadScene(1);
        else if (Input.GetKeyDown("2"))
            LoadScene(2);
    }
#endif

    private void Start()
    {
        if(instance != null)
        {
            gameObject.SetActive(false);
            return;
        }
        instance = this;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }


}
