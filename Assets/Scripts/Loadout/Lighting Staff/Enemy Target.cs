using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    private Rigidbody _rb;
    private float _size = 10;
    public float _speed = 10;
    public Rigidbody Rb => _rb;

    void Update()
    {
        var direction = new Vector3(Mathf.Cos(Time.time * _speed) * _size, Mathf.Sin(Time.time * _speed) * _size);

        _rb.velocity = direction;
    }

    public void Explode() => Destroy(gameObject);
}
