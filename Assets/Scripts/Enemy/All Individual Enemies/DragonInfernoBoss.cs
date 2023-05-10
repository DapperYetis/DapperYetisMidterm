using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonInfernoBoss : HybridEnemy
{
    [Header("---Misc---")]
    [SerializeField]
    private float _attackDelay;

    [Header("---Flight & Falling---")]
    [SerializeField]
    private float _flightHeight = 3f;
    private float _currentHeight;
    [SerializeField]
    private float _fallTime = 0.5f;
    [SerializeField]
    private AnimationCurve _fallCurve;
    [SerializeField]
    private AnimationCurve _swoopCurve;

    [Header("---Attacks---")]
    [SerializeField]
    private EnemyAttackStats _scatterRangedAttack;
    [SerializeField]
    private EnemyAttackStats _scatterRangedAttackScaling;

    protected override void Start()
    {
        base.Start();
        _currentHeight = _flightHeight;
        _agent.baseOffset = _currentHeight;
        _isAttacking = true;
        StartCoroutine(DelayAttack());
    }

    private IEnumerator DelayAttack()
    {
        yield return new WaitForSeconds(5);
        _isAttacking = false;
    }

    protected override void Update()
    {
        if (!_isSetUp) return;

        _anim.SetFloat("Speed", _speed);
        _speed = Mathf.Lerp(_speed, _agent.velocity.normalized.magnitude, Time.deltaTime * _animTransSpeed);

        if (_agent.isActiveAndEnabled)
            _agent.SetDestination(GameManager.instance.player.transform.position);

        FacePlayer();

        if (!_isAttacking)
        {
            _isAttacking = true;
            if (_inAttackRange)
                Melee();
            else
            {
                int attack = Random.Range(0, 3);

                switch (attack)
                {
                    case 0:
                        Shoot();
                        break;
                    case 1:
                        
                        break;
                    case 2:
                        
                        break;
                }
            }
        }
    }

    public override void ScaleEnemy()
    {
        base.ScaleEnemy();

        for (int i = 0; i < EnemyManager.instance.scaleFactorInt - 1; ++i)
        {
            _scatterRangedAttack += _scatterRangedAttackScaling;
        }
    }

    protected override void Die()
    {
        base.Die();
        StartCoroutine(LowerToGround());
    }

    private IEnumerator LowerToGround()
    {
        WaitForEndOfFrame wait = new();
        float startTime = Time.time;
        float inverseFallTime = 1 / _fallTime;
        while(Time.time < startTime + _fallTime)
        {
            _agent.baseOffset = _fallCurve.Evaluate((Time.time - startTime) * inverseFallTime);
            yield return wait;
        }
    }

    // New Attacks
    private void ScatterAttack()
    {
        StartCoroutine(DoScatterAttack());
    }

    private IEnumerator DoScatterAttack()
    {
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(0).Length);
        yield return new WaitForSeconds(_attackDelay);
        _isAttacking = false;
    }
}
