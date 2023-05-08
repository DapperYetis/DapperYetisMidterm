using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField]
    protected int _buildIndex;
    [SerializeField]
    protected bool _isBossRoom;

    void OnTriggerEnter(Collider other)
    {
        if (!_isBossRoom)
            EnemyManager.instance.EnterBossRoom(null);
        else
            EnemyManager.instance.LeaveBossRoom(_buildIndex);
        SceneManage.instance.LoadScene(_buildIndex);
    }
}
