using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSWeapon : MonoBehaviour {

	public int cartridge;
	[SerializeField] internal float damage;
	[SerializeField] internal float fireRate;
	internal int bulletLeft;
	private GameObject muzzleFlash;
	internal int maxBulletCapacity;
	[SerializeField] internal int bulletCapacity;
    [SerializeField] internal float reloadTime;
    [SerializeField] internal float drawTime;

	// Use this for initialization
	void Awake () {
		muzzleFlash = transform.Find("Muzzle Flash").gameObject;
		muzzleFlash.SetActive(false);
		bulletLeft = cartridge;

	}

	private void Start() {
		maxBulletCapacity = bulletCapacity;
	}

	public void Shoot() {
		StartCoroutine(TurnOnMuzzleFlash());
	}

	IEnumerator TurnOnMuzzleFlash() {
		muzzleFlash.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		muzzleFlash.SetActive(false);
	}
}
