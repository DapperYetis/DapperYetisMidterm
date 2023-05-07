using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float _radius = 10f;

    public Vector3 GetPointInRange()
    {
        Vector3 offset = Random.insideUnitCircle * _radius;
        offset.z = offset.y;
        offset.y = 0;
        return transform.position + offset;
    }
}