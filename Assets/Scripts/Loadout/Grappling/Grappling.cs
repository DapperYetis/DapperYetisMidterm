using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{

    [Header("Reference")]
    public PlayerMovementGrappling pm;
    public Transform gunTip, cam;
    public LayerMask whatisGrappleable;
    public LineRenderer lr;

    [Header("Grappling")]
    public float maxGrappleDistance = 500f;
    public float grappleDelayTime = 0.5f;
    public float overshootYAxis = 2f;

    private Vector3 grapplePoint;

    //[Header("Cooldown")]
    //public float grapplingCd = 2.5f;
    //private float grapplingCdTimer;

    private bool grappling;

    private void Start()
    {
        //pm = GetComponent<PlayerMovementGrappling>();

        cam = Camera.main.transform;
        pm = GameManager.instance.player.grappling;
        pm.enabled = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Secondary Support"))
        {
            StartGrapple();
        }
       
        //if (grapplingCdTimer > 0)
        //    grapplingCdTimer -= Time.deltaTime;
    }
    private void LateUpdate()
    {
        if (grappling)
            lr.SetPosition(0, gunTip.position);
    }

    private void StartGrapple()
    {
        //if (grapplingCdTimer > 0)
        //    return;

        grappling = true;

        //pm.freeze = true;

        RaycastHit hit;

        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatisGrappleable))
        {
            grapplePoint = hit.point;

            Invoke(nameof(ExecuteGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(StopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void ExecuteGrapple()
    {
        //pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootYAxis;

        if (grapplePointRelativeYPos < 0)
            highestPointOnArc = overshootYAxis;

        pm.jumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(StopGrapple), 1f);
    }

    public void StopGrapple()
    {
        //pm.freeze = false;

        grappling = false;

        //grapplingCdTimer = grapplingCd;

        lr.enabled = false;
    }

    public bool IsGrappling()
    {
        return grappling;
    }

    public Vector3 getGrapplePoint()
    {
        return grapplePoint;
    }
}
