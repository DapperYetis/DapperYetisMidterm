using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerStats _stats;
    public PlayerStats stats => _stats;

    [Header("-----References------")]
    private CharacterController _controller;

    // Runtime variables
    private bool _wasMoving;
    private bool _isRunning;
    public bool isRunning => _isRunning;
    private Vector3 _playerVelocity;
    public Vector3 playerVelocity => _playerVelocity;
    private int _jumpCountCurrent;
    public int jumpCountCurrent => _jumpCountCurrent;

    // Events
    [HideInInspector]
    public UnityEvent OnMoveStart;
    [HideInInspector]
    public UnityEvent OnMoveStop;
    [HideInInspector]
    public UnityEvent OnSprintStart;
    [HideInInspector]
    public UnityEvent OnSprintStop;
    [HideInInspector]
    public UnityEvent OnJump;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        _playerVelocity = new();
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        PlanarMovement();

        VerticalMovement();

        _controller.Move(_playerVelocity * Time.deltaTime);
    }

    private void PlanarMovement()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            _isRunning = !_isRunning;
            if (_isRunning)
                OnSprintStart.Invoke();
            else
                OnSprintStop.Invoke();
        }

        float speed = _stats.walkSpeed;
        if (_isRunning)
            speed *= _stats.sprintMultiplier;

        _playerVelocity.x = 0;
        _playerVelocity.z = 0;
        _playerVelocity += transform.forward * (Input.GetAxis("Vertical") * speed);
        _playerVelocity += transform.right * (Input.GetAxis("Horizontal") * speed);

        if(!_wasMoving && (playerVelocity - Vector3.up * playerVelocity.y).sqrMagnitude > 0.005)
        {
            OnMoveStart.Invoke();
        }
        else if(_wasMoving && (playerVelocity - Vector3.up * playerVelocity.y).sqrMagnitude <= 0.005)
        {
            OnMoveStop.Invoke();
        }
    }

    private void VerticalMovement()
    {
        _playerVelocity += Vector3.down * (_stats.gravityAcceleration * Time.deltaTime);
        if(_controller.isGrounded && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0;
            _jumpCountCurrent = 0;
        }


        if (Input.GetButtonDown("Jump") && _jumpCountCurrent < _stats.jumpCountMax)
        {
            ++_jumpCountCurrent;
            _playerVelocity.y = Mathf.Sqrt(2 * _stats.jumpHeight * _stats.gravityAcceleration);
            OnJump.Invoke();
        }
    }
}
