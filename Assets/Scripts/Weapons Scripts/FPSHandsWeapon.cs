using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSHandsWeapon : MonoBehaviour {

    public AudioClip shootClip;
    [SerializeField] internal AudioClip clipOutClip;
    [SerializeField] internal AudioClip clipInClip;
    [SerializeField] internal AudioClip drawClip;
	private AudioSource audioManager;
	private GameObject muzzleFlash;

	private Animator anim;

	private string SHOOT = "Shoot";
	private string RELOAD = "Reload";
	
	// Use this for initialization
	void Start () {
		muzzleFlash = transform.Find("MuzzleFlash").gameObject;
		muzzleFlash.SetActive(false);

		audioManager = GetComponent<AudioSource>();
		anim = GetComponent<Animator>();
	}

	public void Shoot() {
		if (audioManager.clip != shootClip) {
			audioManager.clip = shootClip;
		}
//        print("audio played");

        audioManager.Play();
		StartCoroutine(TurnMuzzleFlashOn());
		anim.SetTrigger(SHOOT);
	}

	IEnumerator TurnMuzzleFlashOn() {
		muzzleFlash.SetActive(true);
		yield return new WaitForSeconds(0.05f);
		muzzleFlash.SetActive(false);
	}

	public void Reload() {
		StartCoroutine(PlayReloadSound());
		anim.SetTrigger(RELOAD);
	}

	IEnumerator PlayReloadSound() {
        if (audioManager.clip != clipOutClip) {
            audioManager.clip = clipOutClip;
        }
        audioManager.Play();
		yield return new WaitForSeconds(1.5f);
		if (audioManager.clip != clipInClip) {
			audioManager.clip = clipInClip;
		}
//        print("audio played");
		audioManager.Play();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
