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
    [SerializeField] GameObject _Debuffbar;
    [SerializeField] GameObject _debuffPrefab;
    [SerializeField] GameObject _damageNumberPrefab;
    [SerializeField] GameObject _startLocation;

   
    Vector3 _direction;
    float _remainingHealth;
    EnemyBuffItem debuff;
    EnemyDamage damageScript;
    Dictionary<SOBuff, EnemyBuffItem> _buffs = new();

    #region EnemyHealth
    private void Start()
    {
        _playerCam = Camera.main;
        _enemyAI = transform.parent.GetComponent<EnemyAI>();

        _enemyAI._OnHealthChange.AddListener(UpdateEnemyHealth);
        _enemyAI.OnBuffAdded.AddListener(AddBuff);
        _enemyAI.OnBuffRemoved.AddListener(RemoveBuff);
        _enemyAI.OnEnemyDamagedNumber.AddListener(DamageAnimation);
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
        if(debuff == null) return;
        Destroy(debuff.gameObject);
        _buffs.Remove(buff);
    }
    #endregion

    #region EnemyDamaged

    void  DamageAnimation(float damage)
    {
        damageScript = Instantiate(_damageNumberPrefab, _startLocation.transform).GetComponent<EnemyDamage>();
        damageScript.SetDamage(damage);
        damageScript.PlayDamageAnimation();
    }

    #endregion
}
