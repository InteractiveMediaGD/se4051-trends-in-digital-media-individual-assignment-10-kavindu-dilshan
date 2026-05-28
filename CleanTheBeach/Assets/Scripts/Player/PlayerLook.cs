using System;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
     [SerializeField] private WallRun wr;
     
     [SerializeField] private float sensX;
     [SerializeField] private float sensY;

     [SerializeField] private Transform cam;
     [SerializeField] private Transform orientation;

     private float mouseX;
     private float mouseY;

     private float mulitplier = 0.01f;

     private float xRotation;
     private float yRotation;

     private void Start()
     {
          // Lock and hide cursor for First Person 
          Cursor.lockState = CursorLockMode.Locked;
          Cursor.visible = false;
     }

     private void Update()
     {
          MyInput();
          
          cam.transform.localRotation = Quaternion.Euler(xRotation, yRotation, wr.tilt);
          orientation.transform.rotation = Quaternion.Euler(0, yRotation, 0);
     }

     private void MyInput()
     {
          mouseX = Input.GetAxisRaw("Mouse X");
          mouseY = Input.GetAxisRaw("Mouse Y");

          yRotation += mouseX * sensX * mulitplier;
          xRotation -= mouseY * sensY * mulitplier;

          xRotation = Mathf.Clamp(xRotation, -90f, 90f);
     }
}