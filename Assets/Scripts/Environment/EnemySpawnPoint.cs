using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    private static int MAX_ITERATIONS = 10;
    [SerializeField]
    private float _radius = 10f;

    public Vector3 GetPointInRange()
    {
        Vector3 offset;
        RaycastHit hit;
        int iterations = 0;
        while(iterations <= MAX_ITERATIONS)
        {
            offset = Random.insideUnitCircle * _radius;
            offset.z = offset.y;
            offset.y = 0;
            if(Physics.Raycast(transform.position + offset, Vector3.down, out hit, 1000f))
                return hit.point;

            ++iterations;
        }
        return transform.position;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Handles.color = Color.white;
        Handles.DrawWireDisc(transform.position, transform.up, _radius);
    }
#endif
}
