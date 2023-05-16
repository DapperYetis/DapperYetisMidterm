using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStaff : Staff
{
    [SerializeField]
    protected ProjectileMelee _flame;


    protected override void Update()
    {
        if (Input.GetButtonDown("Primary Fire") && _canUsePrimary)
        {
            _flame.gameObject.SetActive(true);
            _flame.SetStats(_stats.primaryAbility);
            _canUsePrimary = false;
        }
        else if (Input.GetButton("Secondary Fire") && _canUseSecondary && !GameManager.instance.isPaused)
            StartCoroutine(SecondaryFire());
        if (Input.GetButtonUp("Primary Fire") && !_canUsePrimary)
        {
            _flame.gameObject.SetActive(false);
            _canUsePrimary = true;
        }
    }
}
