using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
    [SerializeField]
    private Camera _playerCam;
    private Vector3 _direction;

    private void Start()
    {
        _playerCam = Camera.main;
    }

    void Update()
    {
        LookAtPlayer();
    }

    void LookAtPlayer()
    {
        _direction = _playerCam.transform.position - transform.position;
        _direction.x = 0f;
        _direction.z = 0f;

        transform.LookAt(_playerCam.transform.position - _direction);
        transform.Rotate(0, 180, 0);
    }
}
