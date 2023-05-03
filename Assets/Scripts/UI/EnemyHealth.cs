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

    Vector3 _direction;
    float _remainingHealth;


    private void Start()
    {
        _playerCam = Camera.main;
        _enemyAI = transform.parent.GetComponent<EnemyAI>();

        _enemyAI._OnHealthChange.AddListener(UpdateEnemyHealth);
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

        transform.LookAt( _playerCam.transform.position - _direction);
        transform.Rotate(0, 180, 0);
    }

    void UpdateEnemyHealth()
    {
        _remainingHealth = _enemyAI.GetHealthCurrent() / _enemyAI.GetHealthMax();

        _enemyhealthBar.fillAmount = _remainingHealth;

    }
}
