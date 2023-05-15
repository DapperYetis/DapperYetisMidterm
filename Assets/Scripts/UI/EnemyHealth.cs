using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{

    Camera _playerCam;
    [SerializeField]
    Image _enemyhealthBar;
    EnemyAI _enemyAI;
    [SerializeField]
    GameObject _Debuffbar;
    [SerializeField] 
    GameObject _debuffPrefab;

   
    Vector3 _direction;
    float _remainingHealth;
    EnemyBuffItem debuff;
    Dictionary<SOBuff, EnemyBuffItem> _buffs = new();

    #region EnemyHealth
    private void Start()
    {
        _playerCam = Camera.main;
        _enemyAI = transform.parent.GetComponent<EnemyAI>();

        _enemyAI._OnHealthChange.AddListener(UpdateEnemyHealth);
        _enemyAI._onBuffAdded.AddListener(AddBuff);
        _enemyAI._onBuffRemoved.AddListener(RemoveBuff);
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

    void UpdateEnemyHealth()
    {
        _remainingHealth = _enemyAI.GetHealthCurrent() / _enemyAI.GetHealthMax();

        _enemyhealthBar.fillAmount = _remainingHealth;

    }
    #endregion

    #region EnemyDebuffs
    void AddBuff(SOBuff buff)
    {
        if (!_buffs.ContainsKey(buff))
        {
            debuff = Instantiate(_debuffPrefab, _Debuffbar.transform).GetComponent<EnemyBuffItem>();
            _buffs.Add(buff, debuff);
            debuff.SetBuffUI(buff.icon);
        }
    }

    void RemoveBuff(SOBuff buff)
    {
        Destroy(debuff.gameObject);
        _buffs.Remove(buff);
    }
    #endregion

    #region DamageRecieved
    #endregion
}
