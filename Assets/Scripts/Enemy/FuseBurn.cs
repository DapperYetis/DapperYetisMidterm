using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBurn : MonoBehaviour
{
    AudioSource _audFuse;
    [SerializeField] float _startBurn = 10;
    
    void Start()
    {
        _audFuse = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_audFuse.isActiveAndEnabled)
        {
            if (!_audFuse.isPlaying && (GameManager.instance.player.transform.position - transform.position).magnitude <= _startBurn)
            {
                _audFuse.Play();
            }
            else if (_audFuse.isPlaying && (GameManager.instance.player.transform.position - transform.position).magnitude > 10)
            {
                _audFuse.Stop();
            }
        }
    }
}
