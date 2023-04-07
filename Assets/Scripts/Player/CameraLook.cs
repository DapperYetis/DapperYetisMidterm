using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
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


    // Runtime variables
    private float _currentXRotation;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        float mouseY = (_invertY ? 1 : -1) * Input.GetAxis("Mouse Y") * Time.deltaTime * _sensitivity;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * _sensitivity;

        _currentXRotation += mouseY;
        _currentXRotation = Mathf.Clamp(_currentXRotation, _minimumVertical, _maximumVertical);

        transform.localRotation = Quaternion.Euler(new(_currentXRotation, 0f, 0f));
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
