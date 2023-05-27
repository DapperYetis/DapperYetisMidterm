using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private PlayerStats _stats;
    public PlayerStats stats => _stats;

    [Header("-----References------")]
    private Rigidbody _rb;
    public Rigidbody rb => _rb;
    [SerializeField]
    private Transform _feetPosParent;
    private Transform[] _feetPos;

    [SerializeField]
    private float _groundDist = 0.125f;

    [Header("-----Audio-----")]
    [SerializeField] AudioSource _audio;
    [SerializeField] AudioClip[] _audSteps;
    [SerializeField][Range(0f, 1f)] float _audStepsVol;
    [SerializeField] AudioClip[] _audJump;
    [SerializeField][Range(0f, 1f)] float _audJumpVol;

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
    private bool _toggleSprint => SettingsManager.instance.GetSprintToggle();

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
    [HideInInspector]
    public UnityEvent OnResetMovement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _feetPos = new Transform[_feetPosParent.childCount];
        for(int i = 0; i < _feetPosParent.childCount; ++i)
        {
            _feetPos[i] = _feetPosParent.GetChild(i);
        }
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
            StartCoroutine(PlaySteps());
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
        if(GroundCheck())
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
            if (_audJump.Length > 0)
            {
                _audio.PlayOneShot(_audJump[Random.Range(0, _audJump.Length)], _audJumpVol);
            }
            StartCoroutine(DoJump());
        }
        else if (!_isJumping)
        {
            _playerVelocity.y = _rb.velocity.y;
        }
    }

    private bool GroundCheck()
    {
        foreach(var pos in _feetPos)
        {
            if (Physics.Raycast(pos.position, Vector3.down, _groundDist, Physics.AllLayers ^ (1 << 6)))
                return true;
        }
        return false;
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

    private IEnumerator PlaySteps()
    {
        _wasMoving = true;

        if (_audSteps.Length > 0 && isGrounded)
        {
            _audio.PlayOneShot(_audSteps[Random.Range(0, _audSteps.Length)], _audStepsVol);


            if (!isRunning)
            {
                yield return new WaitForSeconds(0.4f);
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }

            if(_audio.isPlaying && !isGrounded)
            {
                _audio.Stop();
            }
        }

        _wasMoving = false;
    }

    public void ResetMovement()
    {
        _rb.velocity = Vector3.zero;
        OnResetMovement.Invoke();
    }
}
