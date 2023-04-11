using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicCasting : MonoBehaviour
{
    [SerializeField] float _speed;
    [SerializeField] int _timer;
    [SerializeField] GameObject _magicSpell;
    [SerializeField] int _damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, _timer);
    }

    // Update is called once per frame
    void Update()
    {
        _magicSpell.GetComponent<Rigidbody>().velocity = transform.forward * _speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger) return;

        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

        if (damageable != null)
        {
            damageable.Damage(_damage);
        }
        else if (other.transform.gameObject.layer == 8 || other.transform.gameObject.layer == 9)
            Destroy(other);

        Destroy(gameObject);
    }

}
