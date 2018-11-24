using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Ragdoll))]
public class EnemyBehaviour : MonoBehaviour {

	[SerializeField] internal GameObject player;
	[SerializeField] private GameObject bullet;
	[SerializeField] private float emitBulletFrequency = 2f;
	private float countDown = 0f;

	private NavMeshAgent agent;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("FPS Player");
		agent = GetComponent<NavMeshAgent>();
		countDown = emitBulletFrequency;
	}
	
	// Update is called once per frame
	void Update () {
		countDown -= Time.deltaTime;
		if (countDown <= 0f) {
			countDown = emitBulletFrequency;
			GameObject currentBullet = Instantiate(bullet, transform.position+new Vector3(0,0,1), Quaternion.identity);
			
		}
		agent.SetDestination(player.transform.position);
	}

	private void OnCollisionEnter(Collision other) {
        if (other.gameObject.name == "FPS Player")
        {
            other.gameObject.GetComponent<Target>().Die();
        }
    }


}
