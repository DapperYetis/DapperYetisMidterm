using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponsLogic : MonoBehaviour
{
    [Header("----- Weapon Stats -----")]
    [Range(1, 10)][SerializeField] int shootDamage;
    [Range(0.1f, 5)][SerializeField] float shootRate;
    [Range(1, 100)][SerializeField] int shootDist;
    [SerializeField] Transform shootPos;

    [SerializeField] GameObject magicType;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShooting && Input.GetButton("Shoot"))
            StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(magicType, shootPos.position, shootPos.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }
}
