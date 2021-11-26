using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLooks : MonoBehaviour
{
    [SerializeField] private float sensX;
    [SerializeField] private float sensY;

    // Wallrun Script
    [SerializeField] WallRun wallRun; 

    [SerializeField] Transform cam;
    [SerializeField] Transform orientation;

    float mouseX;
    float mouseY;

    float multiplier = 0.01f;

    float xRot;
    float yRot;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    private void Update()
    {
        MouseInput();
        cam.transform.localRotation = Quaternion.Euler(xRot, yRot, wallRun.tilt);
        orientation.transform.rotation = Quaternion.Euler(0, yRot, 0);
    }

    private void MouseInput ()
    {
        mouseX = Input.GetAxisRaw("Mouse X");
        mouseY = Input.GetAxisRaw("Mouse Y");

        yRot += mouseX * sensX * multiplier;
        xRot -= mouseY * sensY * multiplier;

        xRot = Mathf.Clamp(xRot, -90f, 90f);
    }
}
