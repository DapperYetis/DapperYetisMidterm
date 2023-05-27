using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
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
    private float _lowerFieldOfView;
    private float _higherFieldOfView;
    private PlayerMovement _playerMove;
    private float _currentXRotation;

    public void SetUp()
    {
        _cam = GetComponent<Camera>();

        _playerMove = GameManager.instance.playerMovement;
        _playerMove.OnSprintStart.AddListener(HandleSprintStart);
        _playerMove.OnSprintStop.AddListener(HandleSprintStop);
        GameManager.instance.player.inventory.OnItemsChange.AddListener((item) =>
        {
            _higherFieldOfView = _lowerFieldOfView * Mathf.Sqrt(_playerMove.stats.sprintMultiplier > 1 ? _playerMove.stats.sprintMultiplier : 1);
        });
        SettingsManager.instance._onSensitivityChange.AddListener(ChangeSensitivity);
        ChangeSensitivity();

        _lowerFieldOfView = _cam.fieldOfView;
        _higherFieldOfView = _lowerFieldOfView * Mathf.Sqrt(_playerMove.stats.sprintMultiplier > 1 ? _playerMove.stats.sprintMultiplier : 1);
    }

    private void ChangeSensitivity()
    {
        _sensitivity = (SettingsManager.instance.GetSensitivity() * 100);
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
        StopAllCoroutines();
        _cam.fieldOfView = _higherFieldOfView;
        StartCoroutine(TransitionFOV(_lowerFieldOfView, _higherFieldOfView, _fieldOfViewtransitionTime));
    }

    private void HandleSprintStop()
    {
        StopAllCoroutines();
        _cam.fieldOfView = _lowerFieldOfView;
        StartCoroutine(TransitionFOV(_higherFieldOfView, _lowerFieldOfView, _fieldOfViewtransitionTime));
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
