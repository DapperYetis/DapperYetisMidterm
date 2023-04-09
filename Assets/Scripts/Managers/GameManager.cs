using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager instance => _instance;

    [Header("-----References-----")]
    [SerializeField]
    private PlayerController _player;
    public PlayerController player => _player;
    public PlayerMovement playerMovement => _player.movement;

    private void Awake()
    {
        if (_instance)
        {
            gameObject.SetActive(false);
            return;
        }

        _instance = this;

        // Checking for missing references
        if(!_player)
        {
            Debug.LogError("Missing Player reference in GameManager!");
            gameObject.SetActive(false);
        }

        DontDestroyOnLoad(transform.parent.gameObject);
    }
}
