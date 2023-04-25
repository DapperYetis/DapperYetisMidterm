using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleGrappling : MonoBehaviour
{
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private Transform _grappleHook;
    [SerializeField] private Transform _grapplingHookEndPoint;
    [SerializeField] private Transform _handPos;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private LayerMask _grappleLayer;
    [SerializeField] private float _maxGrappleDistance;
    [SerializeField] private float _hookSpeed;
    [SerializeField] private Vector3 _offset;

    private bool isShooting, isGrappling;
    private Vector3 _hookPoint;

    // Start is called before the first frame update
    private void Start()
    {
        isShooting = false;
        isGrappling = false;
        _lineRenderer.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if(_grappleHook.parent == _handPos)
        {
            _grappleHook.localPosition = Vector3.zero;
            _grappleHook.localRotation = Quaternion.Euler(new Vector3(90, 0, 0));

        }
        if(Input.GetButton("Primary Fire"))
        {
            ShootHook();
        }
        if (isGrappling)
        {
            _grappleHook.position = Vector3.Lerp(_grappleHook.position, _hookPoint, _hookSpeed * Time.deltaTime);
            if (Vector3.Distance(_grappleHook.position, _hookPoint) < 0.5f)
            {
                _controller.enabled = false;
                _playerBody.position = Vector3.Lerp(_playerBody.position, _hookPoint - _offset, _hookSpeed * Time.deltaTime);
                if (Vector3.Distance(_playerBody.position, _hookPoint - _offset) < 0.5f)
                {
                    _controller.enabled = true;
                    isGrappling = false;
                    _grappleHook.SetParent(_handPos);
                    _lineRenderer.enabled = false;
                }
            }
        }
    }
    private void LateUpdate()
    {
        if (_lineRenderer.enabled)
        {
            _lineRenderer.SetPosition(0, _grapplingHookEndPoint.position);
            _lineRenderer.SetPosition(1, _handPos.position);
        }
    }

    private void ShootHook()
    {
        if (isShooting || isGrappling)
            return;

        isShooting = true;
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _maxGrappleDistance, _grappleLayer))
        {
            _hookPoint = hit.point;
            isGrappling = true;
            _grappleHook.parent = null;
            _grappleHook.LookAt(_hookPoint);
            _lineRenderer.enabled = true;
        }

        isShooting = false;
    }
}
