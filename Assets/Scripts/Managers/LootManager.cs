using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _lootPrefab;
    [SerializeField]
    private int _lootCount;
    [SerializeField]
    private Bounds _spawningBounds;
    private List<Transform> _lootLocations;
    
    private void Start()
    {
        SetUpStage();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireCube(_spawningBounds.center, _spawningBounds.size);
        if (!Application.isPlaying) return;

        Gizmos.color = Color.black;
        foreach(Transform trans in _lootLocations)
        {
            if(trans != null)
                Gizmos.DrawSphere(trans.position, 1);
        }
    }
#endif

    private void SetUpStage()
    {
        GenerateLootLocations();
    }

    private void GenerateLootLocations()
    {
        _lootLocations = new(_lootCount);
        Vector3 location = new();
        for(int i = 0; i < _lootCount; ++i)
        {
            location.x = Random.Range(_spawningBounds.min.x, _spawningBounds.max.x);
            location.y = _spawningBounds.center.y;
            location.z = Random.Range(_spawningBounds.min.z, _spawningBounds.max.z);

            if(Physics.Raycast(location, Vector3.down, out RaycastHit hitInfo))
            {
                location.y = hitInfo.point.y;
            }

            _lootLocations.Add(Instantiate(_lootPrefab, location, Quaternion.identity).transform);
            _lootLocations[^1].name = $"Loot item ({i + 1})";
        }
    }
}
