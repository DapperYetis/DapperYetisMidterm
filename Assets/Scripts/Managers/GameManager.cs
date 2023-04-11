using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance => _instance;

    private PlayerController _player;
    public PlayerController player => _player;
    public PlayerMovement playerMovement => _player.movement;

    public bool isPaused => UIManager.instance.isPaused;

    private LevelInfo _level;
    public LevelInfo level
    {
        get => _level;
        set => _level = value;
    }

    private void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //DontDestroyOnLoad(transform.parent.gameObject);
    }

    public void ResetMap()
    {
        //player.Heal(player.GetHealthMax());
        //_level?.ResetMap();
        EnemyManager.instance.ResetMap();
        UIManager.instance.TransitionToMainMenu();
        StartCoroutine(RefindPlayer());
    }

    private IEnumerator RefindPlayer()
    {
        _player = null;
        while(true)
        {
            _player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<PlayerController>();
            if (_player != null) break;

            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Player Found!");
        yield return null;
    }
}
