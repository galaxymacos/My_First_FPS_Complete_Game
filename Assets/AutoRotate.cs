using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour {

	[SerializeField] internal float rotateSpeed = 10f;
    
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(new Vector3(0,rotateSpeed*Time.deltaTime,0));
	}
}
