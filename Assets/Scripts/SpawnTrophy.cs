using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrophy : MonoBehaviour {

	[SerializeField] private GameObject[] Spawners;
	[SerializeField] private GameObject[] trophies;

	[SerializeField] private float spawnTrophyInterval = 10f;

	private float timeRemainToSpawn;
	// Use this for initialization
	void Start () {
		timeRemainToSpawn = spawnTrophyInterval;
	}
	
	// Update is called once per frame
	void Update () {
		timeRemainToSpawn -= Time.deltaTime;
		if (timeRemainToSpawn <= 0) {
			timeRemainToSpawn = spawnTrophyInterval;
			Instantiate(trophies[Random.Range(0, trophies.Length)],Spawners[Random.Range(0,Spawners.Length)].transform.position+new Vector3(0,2,0),Quaternion.identity);
		}
	}
}
