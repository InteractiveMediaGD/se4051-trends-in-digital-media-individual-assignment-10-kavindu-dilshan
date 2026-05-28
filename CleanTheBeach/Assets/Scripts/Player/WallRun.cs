using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : MonoBehaviour
{
    [SerializeField] private Transform orientation;

    [Header("Detection")] 
    [SerializeField] private LayerMask _wallrunlayer;
    [SerializeField] private float wallDistance = .5f;
    [SerializeField] private float minimumJumpHeight = 1.5f;

    [Header("Wall Running")] 
    [SerializeField] private float wallRunGravity;
    [SerializeField] private float wallRunJumpForce;

    [Header("Camera")] 
    [SerializeField] private Camera cam;
    [SerializeField] private float fov;
    [SerializeField] private float wallRunFov;
    [SerializeField] private float wallRunFovTime;
    [SerializeField] private float camTilt;
    [SerializeField] private float camTiltTime;
    
    public float tilt { get; private set; }

    private bool wallLeft = false;
    private bool wallRight = false;

    private RaycastHit leftWallHit;
    private RaycastHit rightWallHit;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minimumJumpHeight);
    }

    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftWallHit, wallDistance, _wallrunlayer);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightWallHit, wallDistance, _wallrunlayer);
    }

    private void Update()
    {
        CheckWall();

        if (CanWallRun())
        {
            if(wallLeft || wallRight)
                StartWallRun();
            else
                StopWallRun();
        }
        else
            StopWallRun();
    }

    private void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, wallRunFov, wallRunFovTime * Time.deltaTime);

        if (wallLeft)
            tilt = Mathf.Lerp(tilt, -camTilt, camTiltTime * Time.deltaTime);
        else if(wallRight)
            tilt = Mathf.Lerp(tilt, camTilt, camTiltTime * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Vector3 wallRunJumpDirection = new Vector3();
            if (wallLeft)
                wallRunJumpDirection = transform.up + leftWallHit.normal;
            else if (wallRight)
                wallRunJumpDirection = transform.up + rightWallHit.normal;
            
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(wallRunJumpDirection * (wallRunJumpForce * 100), ForceMode.Force);
        }
    }
    
    private void StopWallRun()
    {
        rb.useGravity = true;
        
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, fov, wallRunFovTime * Time.deltaTime);
        tilt = Mathf.Lerp(tilt, 0, camTiltTime * Time.deltaTime);
    }
}
