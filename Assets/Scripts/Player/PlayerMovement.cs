using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private PlayerStats _stats;

    [Header("-----References------")]
    private CharacterController _controller;

    // Runtime variables
    private bool _isRunning;
    public bool isRunning => _isRunning;
    private Vector3 _playerVelocity;
    public Vector3 playerVelocity => _playerVelocity;
    private int _jumpCountCurrent;
    public int jumpCountCurrent => _jumpCountCurrent;

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
        _playerVelocity.x = 0;
        _playerVelocity.z = 0;
        _playerVelocity += transform.forward * (Input.GetAxis("Vertical") * _stats.runSpeed);
        _playerVelocity += transform.right * (Input.GetAxis("Horizontal") * _stats.runSpeed);
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
        }
    }
}
