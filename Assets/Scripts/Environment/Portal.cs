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
        if (!other.CompareTag("Player")) return;

        if (!_isBossRoom)
            EnemyManager.instance.EnterBossRoom(null);
        else
            EnemyManager.instance.LeaveBossRoom(_buildIndex);

        if (!_isBossRoom && _buildIndex == 3)
            SceneManage.instance.LoadScene(_buildIndex);
        else if(_buildIndex > 3 || PortalBossRoom.totalBosses < _buildIndex)
        {
            UIManager.instance.PauseState();
            UIManager.instance.NextMenu(UIManager.instance.references.winMenu);
            UIManager.instance.WinScreenStats();
        }
        else
            SceneManage.instance.LoadScene(_buildIndex);
    }

    public void SetBuildIndex(int buildIndex)
    {
        _buildIndex = buildIndex;
    }
}
