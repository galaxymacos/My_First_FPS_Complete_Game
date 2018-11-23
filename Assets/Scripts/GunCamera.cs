using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class GunCamera : MonoBehaviour {
    private Camera camera;
	// Use this for initialization
    private float FOV;
    [SerializeField] private float deviation = 15f;
    private float originalFov;
    private float targetFov;
	void Start () {
        camera = GetComponent<Camera>();
        FOV = camera.fieldOfView;
        originalFov = camera.fieldOfView;
        targetFov = originalFov + deviation;
    }

    private bool goingRight = true;

    internal bool isRunning;

    internal bool isWalking;
	// Update is called once per frame
	void Update () {
        if(isRunning)
		ShakeWhenMoving(8f);
        if (isWalking)
            ShakeWhenMoving(5);
    }

    void ShakeWhenMoving(float speed) {
        if (goingRight) {
            if (targetFov-camera.fieldOfView>1)
            {
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, targetFov, speed * Time.deltaTime);
            }
            else
            {
                goingRight = false;
            }

        }
        if (!goingRight) {
            if (camera.fieldOfView-originalFov>1)
            {
                camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, originalFov, speed * Time.deltaTime);
            }
            else {
                goingRight = true;
            }

        }
    }
}
