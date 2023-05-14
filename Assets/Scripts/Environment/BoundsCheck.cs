using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoundsCheck : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        other.transform.position = (from wavePoint in EnemyManager.instance.wavePoints orderby (GameManager.instance.player.transform.position - wavePoint.transform.position).sqrMagnitude select wavePoint.transform.position).First();
    }
}
