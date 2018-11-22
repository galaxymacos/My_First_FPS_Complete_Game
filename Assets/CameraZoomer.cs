using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomer : MonoBehaviour {
	
	[SerializeField] private float zoomValue = 30.0f;
	[SerializeField] private float zoomSpeed = 5f;

	private float originalFOV;
	private float zoomInFOV;

	private Camera camera;
	// Use this for initialization
	void Start () {
		camera = GetComponent<Camera>();
		originalFOV = camera.fieldOfView;
		zoomInFOV = originalFOV - zoomValue;
	}
	
	// Update is called once per frame
	void Update () {
		camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, Input.GetMouseButton(1) ? zoomInFOV : originalFOV, zoomSpeed*Time.deltaTime);
     	}
}
