using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMetalBarRandom : MonoBehaviour {

	private GameObject FpsPlayer;
	private GameObject Collider;

	[SerializeField] private float lifeTime = 3f;

	private bool countdownStart;
	// Use this for initialization
	void Start () {
		transform.Rotate(0,Random.Range(0,180),0);
		FpsPlayer = GameObject.Find("FPS Player");
		Collider = GameObject.Find("CharacterCollider");
	}
	
	// Update is called once per frame
	void Update () {
		if (countdownStart) {
			lifeTime -= Time.deltaTime;
			if (lifeTime <= 0) {
				Destroy(gameObject);
			}
		}
		
	}

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject == Collider) {
			FpsPlayer.GetComponent<Target>().Die();
		}
	}

	private void OnCollisionEnter(Collision other) {
		GetComponent<AudioSource>().Play();
		countdownStart = true;
	}
}
