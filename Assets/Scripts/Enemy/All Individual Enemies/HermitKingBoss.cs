using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HermitKingBoss : HybridEnemy
{
    [SerializeField]
    private EnemyAttackStats _dashAttackStats;
    public EnemyAttackStats dashAttackStats => _dashAttackStats;
    [SerializeField]
    private EnemyAttackStats _dashAttackScaling;
    public EnemyAttackStats dashAttackStatsScaling => _dashAttackScaling;
    [SerializeField]
    private HermitKingDash _dashLogic;
    private bool _isDashing;

    [SerializeField]
    private EnemyAttackStats _sprayAttackStats;
    public EnemyAttackStats sprayAttack => _sprayAttackStats;
    [SerializeField]
    private EnemyAttackStats _sprayAttackScaling;
    public EnemyAttackStats sprayAttackScaling => _sprayAttackScaling;

    protected override void Start()
    {
        UIManager.instance.TurnOnBossHealthBar();
        base.Start();
        _isAttacking = true;
        StartCoroutine(DelayAttack());
        OnEnemyDamaged.AddListener(OnTakeDamage);
        OnBossDied.AddListener(BossDied);
        UIManager.instance.SetBossHealthbar(GetHealthMax(), GetHealthCurrent());
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

        if (_agent.isActiveAndEnabled && !_isDashing)
            _agent.SetDestination(GameManager.instance.player.transform.position);

        FacePlayer();

        if (!_isAttacking)
        {
            _isAttacking = true;
            if (_inAttackRange)
                EnemyManager.instance.QueueAttack(Melee, () => isActiveAndEnabled ? Mathf.FloorToInt(_playerDir.magnitude) : int.MaxValue, this);
            else
            {
                int attack = Random.Range(0, 3);
                // Can add attack queueing later, but keeping boss out of queue may also be appropriate
                switch (attack)
                {
                    case 0:
                        Shoot();
                        break;
                    case 1:
                        Dash();
                        break;
                    case 2:
                        SprayShoot();
                        break;
                }
            }
        }

        CheckBuffs();
    }

    public override void ScaleEnemy()
    {
        base.ScaleEnemy();

        for (int i = 0; i < EnemyManager.instance.scaleFactorInt - 1; ++i)
        {
            _dashAttackStats += _dashAttackScaling;
            _sprayAttackStats += _sprayAttackScaling;
        }
    }

    private void SprayShoot()
    {
        //Debug.Log("Spray");
        StartCoroutine(DoSprayShoot());
    }

    private IEnumerator DoSprayShoot()
    {
        _anim.SetTrigger("Shoot");
        Quaternion rot;

        int projectileCount = Mathf.CeilToInt(Mathf.Log(_sprayAttackStats.speed));
        for (int i = 0; i < _sprayAttackStats.positions.Length; ++i)
        {
            for (int j = 0; j < projectileCount; ++j)
            {
                rot = Quaternion.LookRotation(VectorToPlayer(_sprayAttackStats) * 0.5f);
                if (Mathf.Abs(Quaternion.Angle(rot, Quaternion.LookRotation(VectorToPlayer(_sprayAttackStats)))) >= 60)
                    rot = Quaternion.LookRotation(VectorToPlayer(_sprayAttackStats));
                rot = Quaternion.RotateTowards(rot, Random.rotation, _sprayAttackStats.variance);
                Instantiate(_sprayAttackStats.prefab, _sprayAttackStats.positions[i].position, rot).GetComponent<Projectile>().SetStats(_sprayAttackStats);
            }
        }

        yield return new WaitForSeconds(_sprayAttackStats.rate);
        _isAttacking = false;
    }

    private void Dash()
    {
        StartCoroutine(DoDash());
    }

    private IEnumerator DoDash()
    {
        _isDashing = true;
        _dashLogic.enabled = true;
        float origSpeed = _agent.speed;
        _agent.speed = _dashAttackStats.speed;
        _anim.SetTrigger("Attack");

        yield return new WaitForSeconds(_dashAttackStats.lifetime);

        _agent.speed = origSpeed;
        _dashLogic.enabled = false;
        _isDashing = false;

        yield return new WaitForSeconds(_dashAttackStats.rate);

        _isAttacking = false;
    }

    public void OnTakeDamage()
    {
        UIManager.instance.SetBossHealthbar(GetHealthMax(), GetHealthCurrent());
    }

    public void BossDied()
    {
        UIManager.instance.TurnOffBossHealthBar();
    }
}
