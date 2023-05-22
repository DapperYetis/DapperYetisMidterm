using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField]
    private float _radius = 10f;

    public Vector3 GetPointInRange()
    {
        Vector3 offset;
        RaycastHit hit;
        while(true)
        {
            offset = Random.insideUnitCircle * _radius;
            offset.z = offset.y;
            offset.y = 0;
            if(Physics.Raycast(transform.position + offset, Vector3.down, out hit, _radius))
                return hit.point;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.up, _radius);
    }
#endif
}
