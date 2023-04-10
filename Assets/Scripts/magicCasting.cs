using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magicCasting : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] int timer;
    [SerializeField] GameObject magicSpell;
    [SerializeField] int damage;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        magicSpell.GetComponent<Rigidbody>().velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

}
