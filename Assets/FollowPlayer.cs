using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
	[SerializeField] private Transform player;

	private void LateUpdate() {
		transform.position = new Vector3(player.position.x,transform.position.y,player.position.z);
	}
}
