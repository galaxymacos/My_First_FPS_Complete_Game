using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	private AudioSource audioSource;
	
	[SerializeField] internal AudioClip deagleClipOut;
	[SerializeField] internal AudioClip deagleClipin;
	[SerializeField] internal AudioClip Ak47ClipOut;
	[SerializeField] internal AudioClip Ak47ClipIn;
	[SerializeField] internal AudioClip M4A1ClipOut;
	[SerializeField] internal AudioClip M4A1ClipIn;
	[SerializeField] internal AudioClip enemySpawn;
	
	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void PlayAudioClip(AudioClip clip) {
		audioSource.clip = clip;
		audioSource.Play();
	}
}
