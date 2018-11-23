using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour {

	[SerializeField] private GameObject objectToSpawn;

	[SerializeField] private GameObject[] SpawnPoints;

	[SerializeField] internal float spawnInterval = 3f;

	private float spawnTimeRemain = 0f;

	private AudioSource _audioSource;
	// Use this for initialization
	void Start () {
		spawnTimeRemain = spawnInterval;
		_audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimeRemain -= Time.deltaTime;
		if (spawnTimeRemain <= 0f) {
			spawnTimeRemain = spawnInterval;
			int random = Random.Range(0, SpawnPoints.Length - 1);
			_audioSource.Play();
			GameObject robot = Instantiate(objectToSpawn, SpawnPoints[random].transform.position, Quaternion.identity);
			robot.GetComponent<EnemyBehaviour>().player = GameObject.Find("FPS Player");
		}
	}
}
