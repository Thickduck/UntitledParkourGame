using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] Transform orientation;

    [Header("Detection of Wallrun")]
    [SerializeField] float wallDistance = 0.5f;
    [SerializeField] float minJump = 1.5f;

    [Header("Wallrunning")]
    [SerializeField] float wallRunGravity;
    [SerializeField] float wallRunForce;

    [Header("Camera")]
    [SerializeField] Camera cam;
    [SerializeField] float fov;
    [SerializeField] float wallRunFov;
    [SerializeField] float wallRunFovTime;
    [SerializeField] float camTilt;
    [SerializeField] float camTiltTime;

    public float tilt { get; private set; }


    // Raycast hits
    RaycastHit leftWall;
    RaycastHit rightWall;

    bool wallLeft = false;
    bool wallRight = false;

    Rigidbody rb;


    //Default methods

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Methods

    bool canWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJump);
    }

    void wallCheck()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWall, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWall, wallDistance);
    }

    private void Update()
    {
        wallCheck();

        if (canWallRun())
        {
            if (wallLeft)
            {
                wallRunStart();
            }
            else if (wallRight)
            {
                wallRunStart();
            }
            else
            {
                wallRunStop();
            }
        }
        else
        {
            wallRunStop();
        }
    }

    private void wallRunStart()
    {
        rb.useGravity = false;
        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
        {
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        }
        else if (wallRight)
        {
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (wallLeft)
            {
                Vector3 wallRunJumpDir = transform.up + leftWall.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDir * wallRunForce * 100, ForceMode.Force);
            }
            else if (wallRight)
            {
                Vector3 wallRunJumpDir = transform.up + rightWall.normal;
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                rb.AddForce(wallRunJumpDir * wallRunForce * 100, ForceMode.Force);
            }
        }
    }

    private void wallRunStop()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        rb.useGravity = true;
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
