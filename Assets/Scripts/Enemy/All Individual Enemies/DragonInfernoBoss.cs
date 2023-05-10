using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonInfernoBoss : HybridEnemy
{
    [Header("---Misc---")]
    [SerializeField]
    private float _attackDelay;
    // Can be made into an enum later for better readability and safety
    private int _phase = 1;
    [SerializeField, Range(0, 1)]
    private float _phaseTwoThreshold;
    [SerializeField, Range(0, 1)]
    private float _phaseThreeThreshold;

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
        yield return new WaitForSeconds(3);
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
                switch (_phase)
                {
                    case 1:
                        Shoot(_secondaryAttackStats);
                        break;
                    case 2:
                        _anim.SetTrigger("AttackScatter");
                        break;
                    case 3:
                        Shoot(_secondaryAttackStats);
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

    public override void Damage(float amount, (SOBuff buff, int amount)[] buffs)
    {
        base.Damage(amount, buffs);
        if (_HPCurrent < _phaseThreeThreshold * _baseStats.HPMax && _phase != 3)
        {
            _phase = 3;
            //Heal(_baseStats.HPMax * 0.05f);
        }
        else if (_HPCurrent < _phaseTwoThreshold * _baseStats.HPMax && _phase != 2)
        {
            _phase = 2;
            //Heal(_baseStats.HPMax * 0.1f);
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
        while (Time.time < startTime + _fallTime)
        {
            _agent.baseOffset = _fallCurve.Evaluate((Time.time - startTime) * inverseFallTime);
            yield return wait;
        }
    }

    // New Attacks
    // Called via anim event
    private void ScatterAttack()
    {
        StartCoroutine(DoScatterAttack());
    }

    private IEnumerator DoScatterAttack()
    {
        for (int i = 0; i < Mathf.CeilToInt(1 / _scatterRangedAttack.rate); ++i)
        {
            Shoot(_scatterRangedAttack);
        }
        yield return new WaitForSeconds(_anim.GetCurrentAnimatorClipInfo(0).Length);
        yield return new WaitForSeconds(_attackDelay);
        _isAttacking = false;
    }

    private void Shoot(EnemyAttackStats stats)
    {

        if (this == null || !isActiveAndEnabled) return;

        StartCoroutine(FireShot(stats));
    }

    protected IEnumerator FireShot(EnemyAttackStats stats)
    {
        _anim.SetTrigger("Shoot");
        if (stats._attackAudio.Length > 0)
            _aud.PlayOneShot(stats._attackAudio[Random.Range(0, stats._attackAudio.Length)], stats._attackAudioVol);
        Debug.Log($"{name} played a sound");
        Quaternion rot = Quaternion.LookRotation(GameManager.instance.player.transform.position - stats.positions[0].position * 0.5f);
        if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(GameManager.instance.player.transform.position - stats.positions[0].position))) >= 60)
            rot = Quaternion.LookRotation(GameManager.instance.player.transform.position - stats.positions[0].position);
        rot = Quaternion.RotateTowards(rot, Random.rotation, stats.variance * GameManager.instance.player.movement.speedRatio);
        for (int i = 0; i < stats.positions.Length; i++)
        {
            Instantiate(stats.prefab, stats.positions[i].position, rot).GetComponent<Projectile>().SetStats(stats);
        }

        _hasCompletedAttack = true;
        yield return new WaitForSeconds(stats.rate);
        _isAttacking = false;
    }
}
