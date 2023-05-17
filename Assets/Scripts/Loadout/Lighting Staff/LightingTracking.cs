using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LightingTracking : Projectile
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private EnemyTarget _target;
    [SerializeField] private GameObject _explostionPrefab;

    [SerializeField] private float _speed = 15;
    [SerializeField] private float _rotateSpeed = 95;

    [Header("-----Prediction-----")]
    [SerializeField] private float _maxDistancePredict = 100;
    [SerializeField] private float _minDistancePredict = 5;
    [SerializeField] private float _maxTimePrediction = 5;
    [SerializeField] private Vector3 _standardPrediction, _deviatedPrediction;

    [Header("-----Deviation-----")]
    [SerializeField] private float _deviationAmount = 50;
    [SerializeField] private float _deviationSpeed = 50;

    private List<IDamageable> _previouslyHit = new();

    private void FixedUpdate()
    {
        _rb.velocity = transform.forward * _speed;

        var leadTimePrecentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict, Vector3.Distance(transform.position, _target.transform.position));

        PredictMovement(leadTimePrecentage);

        AddDeviation(leadTimePrecentage);

        RotateRocket();
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

        _standardPrediction = _target.Rb.position + _target.Rb.velocity * predictionTime;
    }

    private void AddDeviation(float leadTimePrecentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

        var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePrecentage;

        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;

        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    //private void OnCollitionEnter(Collision collision)
    //{
    //    if (_explostionPrefab)
    //        Instantiate(_explostionPrefab, transform.position, Quaternion.identity);

    //    if (collision.gameObject.TryGetComponent<IDamageable>(out var damageable) && !_previouslyHit.Contains(damageable))
    //    {
    //        var buffs = (from buff in _stats.targetBuffs select (buff, 1)).ToArray();
    //        if (_hasCrit)
    //            damageable.Damage(_stats.directDamage, buffs);
    //        else
    //            damageable.Damage(_stats.directDamage);
    //        _previouslyHit.Add(damageable);

    //        OnHit?.Invoke(this, damageable);
    //    }

    //    Destroy(gameObject);
    //}

    /*
        public GameObject _Prefab;
        [SerializeField] private Transform _spawnPoint;
        private GameObject target;
        public float speed = 1f;

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Q))
            {
                GameObject _lighting = Instantiate(_Prefab, _spawnPoint.transform.position, _Prefab.transform.rotation);
                _lighting.transform.LookAt(target.transform);
                StartCoroutine(SendHoming(_lighting));
            }
        }

        public IEnumerator SendHoming(GameObject rocket)
        {
            while (Vector3.Distance(target.transform.position, rocket.transform.position) > 0.3f)
            {
                rocket.transform.position += (target.transform.position - rocket.transform.position).normalized * speed * Time.deltaTime;
                rocket.transform.LookAt(target.transform);
                yield return null;
            }
            Destroy(rocket);
        }
    }*/

    /*public Transform LightingTarget;
    public Rigidbody LightingRb;

    public float turnSpeed = 1f;
    public float flySpeed = 10f;

    private Transform lightingLocalTrans;

    private void Start()
    {
        lightingLocalTrans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

        }
    }

    private void FixedUpdate()
    {
        if(!LightingTarget)
            return;

        LightingRb.velocity = lightingLocalTrans.forward * flySpeed;

        var lightingTargetRot = Quaternion.LookRotation(LightingTarget.position - lightingLocalTrans.position);

        LightingRb.MoveRotation(Quaternion.RotateTowards(lightingLocalTrans.rotation, lightingTargetRot, turnSpeed));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody ememyRb = collision.gameObject.GetComponent<Rigidbody>();
            if (ememyRb)
            {
                ememyRb.AddForceAtPosition(Vector3.up * 1000f, ememyRb.position);
            }
        }
    }*/
}
