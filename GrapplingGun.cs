using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingGun : MonoBehaviour
{
    private LineRenderer lr;
    private Vector3 grapplePoint;

    public LayerMask whatIsGrap;

    public Transform cam;
    public Transform gunTip;
    public Transform player; 

    private float maxDist = 100f;

    private SpringJoint joint;

    [Header("Spring Settings")]
    [SerializeField] float springValue = 4.5f;
    [SerializeField] float damperValue = 7f;
    [SerializeField] float massValue = 4.5f; 

    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
        } else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
        }
    }

    private void LateUpdate()
    {
        Rope();
    }

    void StartGrapple ()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxDist, whatIsGrap))
        {
            grapplePoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();

            float distance = Vector3.Distance(player.position, grapplePoint);
            
            // joint conf
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = grapplePoint;
            
            joint.maxDistance = distance * 0.8f;
            joint.minDistance = distance * 0.25f; 

            joint.massScale = massValue;
            joint.spring = springValue;
            joint.damper = damperValue;

            lr.positionCount = 2; 

        }
    }

    void Rope ()
    {
        if (!joint)
        {
            return;
        }
        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, grapplePoint);
    }


    void StopGrapple() 
    {
        lr.positionCount = 0;
        Destroy(joint);
    }

    public bool isGrappling ()
    {
        return joint != null;
    }
    public Vector3 getGrapplingPoint ()
    {
        return grapplePoint;
    }

}
