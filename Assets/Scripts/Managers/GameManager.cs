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

    private void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        DontDestroyOnLoad(transform.parent.gameObject);
    }
}
