using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoundsCheck : MonoBehaviour
{
    private static float HEIGHT_OFFSET = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (!(other.CompareTag("Player") || other.CompareTag("TeleportBeacon"))) return;

        other.transform.position = (from wavePoint in EnemyManager.instance.wavePoints orderby (GameManager.instance.player.transform.position - wavePoint.transform.position).sqrMagnitude select wavePoint.transform.position).First() + Vector3.up * HEIGHT_OFFSET;
        GameManager.instance.player.movement.ResetMovement();
    }
}
