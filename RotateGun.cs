using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGun : MonoBehaviour
{
    public GrapplingGun grapple;

    private Quaternion desiredRot;
    private float speedRot = 5f;

    private void Update()
    {
        if (!grapple.isGrappling())
        {
            desiredRot = transform.parent.rotation;
        }
        else
        {
            desiredRot = Quaternion.LookRotation(grapple.getGrapplingPoint() - transform.position);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, desiredRot, Time.deltaTime  * speedRot);
    }
}
