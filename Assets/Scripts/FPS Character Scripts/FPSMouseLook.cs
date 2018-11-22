using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSMouseLook : MonoBehaviour {
    public enum RotationAxes {
        MouseX,
        MouseY
    }

    public RotationAxes axes = RotationAxes.MouseY;

    private float currentSensitivityX = 1.5f;
    private float currentSensitivityY = 1.5f;

    private float sensitivityX = 1.5f;
    private float sensitivityY = 1.5f;

    private float rotationX, rotationY;

    private float minimumX = -360f;
    private float maximumX = 360f;

    private float minimumY = -360f;
    private float maximumY = 360f;

    private Quaternion originalRotation;

    private float mouseSensitivity = 1.7f;





    // Use this for initialization
    void Start() {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update() {
    }

    void FixedUpdate() { }

    void LateUpdate()
    {
        HandleRotation();
    }

    float ClampAngle(float angle, float min, float max) {
        if (angle < -360f) {
            angle += 360f;
        }

        if (angle > 360f) {
            angle -= 360f;
        } 

        return Mathf.Clamp(angle, min, max);
    }

    void HandleRotation() {
        if (currentSensitivityX != mouseSensitivity || currentSensitivityY != mouseSensitivity) {
            currentSensitivityX = currentSensitivityX = mouseSensitivity;
        }

        sensitivityX = currentSensitivityX;
        sensitivityY = currentSensitivityY;

        if (axes == RotationAxes.MouseX) {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            transform.localRotation = originalRotation * xQuaternion;
        }

        if (axes == RotationAxes.MouseY) {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(-rotationY, Vector3.right);
            transform.localRotation = originalRotation * yQuaternion;
        }
    }
}