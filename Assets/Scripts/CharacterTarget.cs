using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTarget : MonoBehaviour {

	[SerializeField] private GameObject FPSPlayer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HitByBullet() {
        FPSPlayer.GetComponent<Target>().TakeDamage(10);
	}
	
	
}
