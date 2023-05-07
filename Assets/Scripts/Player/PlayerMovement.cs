using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerStats _stats;
    public PlayerStats stats => _stats;

    [Header("-----References------")]
    private Rigidbody _rb;
    public Rigidbody rb => _rb;
    [SerializeField]
    private Transform _footPos;

    // Runtime variables
    private bool _wasMoving;
    private bool _isRunning;
    public bool isRunning => _isRunning;
    private Vector3 _playerVelocity;
    public Vector3 playerVelocity => _playerVelocity;
    public float speedRatio => _playerVelocity.magnitude / _stats.walkSpeed * (_isRunning ? _stats.sprintMultiplier : 1f);
    private int _jumpCountCurrent;
    private bool _isJumping;
    public bool isJumping => _isJumping;
    public int jumpCountCurrent => _jumpCountCurrent;
    private bool _isGrounded;
    public bool isGrounded => _isGrounded;
    private bool _toggleSprint => UIManager.instance.GetSprintToggle();

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
    [HideInInspector]
    public UnityEvent OnLand;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _playerVelocity = new();
    }

    private void Update()
    {
        Movement();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _playerVelocity;
    }

    public void SetPosition(Vector3 pos) => _rb.position = pos;

    private void Movement()
    {
        PlanarMovement();

        VerticalMovement();
    }

    private void PlanarMovement()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (_toggleSprint)
                _isRunning = !_isRunning;
            else
                _isRunning = true;

            if (_isRunning)
            {
                OnSprintStart.Invoke();
            }
            else if(_toggleSprint)
                OnSprintStop.Invoke();
        }
        else if ((Input.GetButtonUp("Sprint") && !_toggleSprint) || (_isRunning && Input.GetAxis("Vertical") < 0.001f))
        {
            _isRunning = false;
            OnSprintStop.Invoke();
        }

        float speed = _stats.walkSpeed;
        if (_isRunning)
            speed *= _stats.sprintMultiplier;

        _playerVelocity.x = 0;
        _playerVelocity.z = 0;
        _playerVelocity += transform.forward * (Input.GetAxis("Vertical") * speed);
        _playerVelocity += transform.right * (Input.GetAxis("Horizontal") * speed);

        if (!_wasMoving && (playerVelocity - Vector3.up * playerVelocity.y).sqrMagnitude > 0.005)
        {
            _wasMoving = true;
            OnMoveStart.Invoke();
        }
        else if (_wasMoving && (playerVelocity - Vector3.up * playerVelocity.y).sqrMagnitude <= 0.005)
        {
            _wasMoving = false;
            OnMoveStop.Invoke();
        }
    }

    private void VerticalMovement()
    {
        if(Physics.Raycast(_footPos.position, Vector3.down, 0.125f, Physics.AllLayers ^ (1 << 6)))
        {
            if (!_isGrounded)
                OnLand.Invoke();

            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }

        if (_isGrounded && !Input.GetButton("Jump"))
        {
            _jumpCountCurrent = 0;
        }


        if (Input.GetButtonDown("Jump") && !_isJumping && _jumpCountCurrent < _stats.jumpCountMax)
        {
            StartCoroutine(DoJump());
        }
        else if (!_isJumping)
        {
            _playerVelocity.y = _rb.velocity.y;
        }
    }

    private IEnumerator DoJump()
    {
        _isJumping = true;
        _rb.useGravity = false;
        ++_jumpCountCurrent;

        float startTime = Time.time;
        float minInitialVelocity = Mathf.Sqrt(2 * _stats.jumpHeightMin * _stats.gravityAcceleration);
        float maxInitialVelocity = Mathf.Sqrt(2 * _stats.jumpHeightMax * _stats.gravityAcceleration);
        float totalVelocityIncrement = maxInitialVelocity - minInitialVelocity;
        WaitForEndOfFrame wait = new();

        _playerVelocity.y = minInitialVelocity;
        OnJump.Invoke();


        while (Time.time < startTime + _stats.jumpInputTime)
        {
            if (Input.GetButton("Jump"))
                _playerVelocity.y = minInitialVelocity + totalVelocityIncrement * (Time.time - startTime) / _stats.jumpInputTime;
            yield return wait;
        }

        _isJumping = false;
        _rb.useGravity = true;
    }

    public void SetStats(PlayerStats newStats)
    {
        _stats = newStats;
        Physics.gravity = Vector3.down * _stats.gravityAcceleration;
    }
}
