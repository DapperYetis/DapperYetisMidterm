using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;
    public UnityEvent onTransitionToMain;

#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            LoadScene(0);
        else if (Input.GetKeyDown(KeyCode.Alpha1))
            LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
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
        if(scene == 0)
            onTransitionToMain.Invoke();
    }


}
