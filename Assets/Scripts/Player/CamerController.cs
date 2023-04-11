using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CamerController : MonoBehaviour
{
    [Header("------Constraints------")]
    
    [SerializeField]
    private float _minimumVertical;
    [SerializeField]
    private float _maximumVertical;


    [Header("------Tuning------")]

    [SerializeField]
    private float _sensitivity;
    [SerializeField]
    private bool _invertY;
    [SerializeField]
    private float _fieldOfViewtransitionTime;


    // Runtime variables
    private Camera _cam;
    private float _fieldOfView;
    private PlayerMovement _playerMove;
    private float _currentXRotation;

    private void Start()
    {
        _cam = GetComponent<Camera>();
        
        _playerMove = GameManager.instance.playerMovement;
        _playerMove.OnSprintStart.AddListener(HandleSprintStart);
        _playerMove.OnSprintStop.AddListener(HandleSprintStop);
    }

    private void Update()
    {
        if (Cursor.lockState != CursorLockMode.Locked) return;

        float mouseY = (_invertY ? 1 : -1) * Input.GetAxis("Mouse Y") * Time.deltaTime * _sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * _sensitivity;

        _currentXRotation += mouseY;
        _currentXRotation = Mathf.Clamp(_currentXRotation, _minimumVertical, _maximumVertical);

        transform.localRotation = Quaternion.Euler(new(_currentXRotation, 0f, 0f));
        transform.parent.Rotate(Vector3.up * mouseX);
    }

    private void HandleSprintStart()
    {
        _fieldOfView = _cam.fieldOfView;
        StartCoroutine(TransitionFOV(_fieldOfView, _fieldOfView * Mathf.Sqrt(_playerMove.stats.sprintMultiplier), _fieldOfViewtransitionTime));
    }

    private void HandleSprintStop()
    {
        StartCoroutine(TransitionFOV(_cam.fieldOfView, _fieldOfView, _fieldOfViewtransitionTime));
    }

    private IEnumerator TransitionFOV(float start, float end, float time)
    {
        WaitForEndOfFrame wait = new WaitForEndOfFrame();
        
        float endTime = Time.time + time;
        while(endTime >= Time.time)
        {
            _cam.fieldOfView = Mathf.Lerp(start, end, 1 - (endTime - Time.time) / time);
            yield return wait;
        }

        _cam.fieldOfView = end;
    }
}
