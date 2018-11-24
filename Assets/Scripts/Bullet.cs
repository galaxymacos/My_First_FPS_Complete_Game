using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

	private GameObject player;
	[SerializeField] float flyingSpeed = 10f;

	private Vector3 direction;
	// Use this for initialization
	void Start () {
		player = GameObject.Find("FPS Player");
		direction = player.transform.position - transform.position+new Vector3(0,player.GetComponent<CharacterController>().height/2,0);
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.Normalize(direction)*flyingSpeed*Time.deltaTime);
	}


	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.name == "BulletCollider") {
			other.gameObject.GetComponent<CharacterTarget>().HitByBullet();
            Destroy(gameObject);
        }
    }
}
