using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleSwing : MonoBehaviour
{
    [Header("References")]
    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatisGrappleable;
    public PlayerMovementGrappling pm;

    [Header("Swinging")]
    [SerializeField] private float maxSwingDistance = 70f;
    private Vector3 swingPoint;
    private SpringJoint joint;

    [Header("Prediction")]
    public RaycastHit predictionHit;
    public Transform predictionPoint;
    public float predictionSphereCastRadius;

    [Header("OdmGear")]
    public Transform orientation;
    public Rigidbody rb;
    public float horizontalThrustForce;
    public float forwardThrustForce;
    public float extendCableSpeed;

    private void Start()
    {
        predictionPoint = Instantiate(predictionPoint.gameObject).transform;
        cam = Camera.main.transform;
        player = GameManager.instance.player.transform;
        pm = GameManager.instance.player.grappling;
        pm.enabled = true;
        orientation = GameManager.instance.player.grappling.orintation;
        rb = GameManager.instance.player.movement.rb;

    }
    void Update()
    {
        CheckForSwingPoints();

        if (joint != null)
            OdmGearMovement();
    }
    private void LateUpdate()
    {
        DrawRope();
    }
    public void StartSwing()
    {
        if (predictionHit.point == Vector3.zero)
            return;

        pm.swinging = true;

        swingPoint = predictionHit.point;
        joint = player.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = swingPoint;

        float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

        joint.maxDistance = distanceFromPoint * 0.8f;
        joint.minDistance = distanceFromPoint * 0.25f;

        joint.spring = 4.5f;
        joint.damper = 7f;
        joint.massScale = 4.5f;

        lr.positionCount = 2;
        currentGrapplePosition = gunTip.position;
    }
    public void StopSwing()
    {
        pm.swinging = false;
        lr.positionCount = 0;
        Destroy(joint);
        joint = null;
    }
    private Vector3 currentGrapplePosition;
    void DrawRope()
    {
        if (!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePosition);
    }
    private void OdmGearMovement()
    {
        if (Input.GetKey(KeyCode.D))
            rb.AddForce(orientation.right * horizontalThrustForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            rb.AddForce(-orientation.right * horizontalThrustForce * Time.deltaTime);
        if (Input.GetKey(KeyCode.W))
            rb.AddForce(orientation.forward * forwardThrustForce * Time.deltaTime);

        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 directionToPoint = swingPoint - transform.position;
            rb.AddForce(directionToPoint.normalized * forwardThrustForce * Time.deltaTime);

            float distanceFromPoint = Vector3.Distance(transform.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            float extendedDistanceFromPoint = Vector3.Distance(transform.position, swingPoint) + extendCableSpeed;

            joint.maxDistance = extendedDistanceFromPoint * 0.8f;
            joint.minDistance = extendedDistanceFromPoint * 0.25f;
        }
    }
    private void CheckForSwingPoints()
    {
        //if (joint != null)
        //    return;
        RaycastHit sphereCastHit;
        Physics.SphereCast(cam.position, predictionSphereCastRadius, cam.forward, out sphereCastHit, maxSwingDistance, whatisGrappleable);

        RaycastHit raycastHit;
        Physics.Raycast(cam.position, cam.forward, out raycastHit, maxSwingDistance, whatisGrappleable);

        Vector3 realHitPoint = Vector3.zero;

        if (raycastHit.point != Vector3.zero)
            realHitPoint = raycastHit.point;
        else if (sphereCastHit.point != Vector3.zero)
            realHitPoint = sphereCastHit.point;
        else
            realHitPoint = Vector3.zero;

        if (realHitPoint != Vector3.zero)
        {

            predictionPoint.gameObject.SetActive(true);
            predictionPoint.position = realHitPoint;
        }
        else
        {
            predictionPoint.gameObject.SetActive(false);
        }

        predictionHit = raycastHit.point == Vector3.zero ? sphereCastHit : raycastHit;
    }
}
