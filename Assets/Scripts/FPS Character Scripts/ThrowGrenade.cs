using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour {

	[SerializeField] private GameObject grenade;

	[SerializeField] private float throwForce = 10f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.G)) {
			GameObject grenadeSpawned = Instantiate(grenade, transform.position, Quaternion.identity);
			grenadeSpawned.GetComponent<Rigidbody>().AddForce(transform.TransformDirection(Vector3.forward)*throwForce,ForceMode.Impulse);
		}
	}
}
