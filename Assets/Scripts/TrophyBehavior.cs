using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrophyBehavior : MonoBehaviour {
    [SerializeField] private float rotateSpeed = 100f;
    private GameObject FPSPlayer;
    

    [SerializeField] private float liveTime = 8f;

    [SerializeField] private string name;

    private AudioSource _audioSource;
    [SerializeField] private AudioClip healingSound;
    [SerializeField] private AudioClip addBulletSound;
    [SerializeField] private AudioClip existingSound;
    

    // Use this for initialization
    void Start() {
        _audioSource = GetComponent<AudioSource>();
        FPSPlayer = GameObject.Find("FPS Player");
    }

    // Update is called once per frame
    void Update() {
        transform.Rotate(new Vector3(0, rotateSpeed * Time.deltaTime, 0));
        liveTime -= Time.deltaTime;
        if (liveTime <= 0f) {
            Destroy(gameObject);
        }
    }

    IEnumerator PlayHealingSound() {    // The volume is too low, so we have to delay it in order to prevent it from being covered by the bullet fire sound
        yield return new WaitForSeconds(0.5f);
        if (_audioSource.clip != healingSound)
            _audioSource.clip = healingSound;
        _audioSource.Play();
    }

    public void PlayTrophy() {
        if (name == "Heal") {
            FPSPlayer.GetComponent<Target>().currentHp = FPSPlayer.GetComponent<Target>().maxHp;    // Recover hp
            StartCoroutine(PlayHealingSound());
        }
        else if (name == "MoreBullet") {
            var fpsController = FPSPlayer.GetComponent<FPSController>();
            fpsController.currentWeapon.bulletCapacity += fpsController.currentWeapon.maxBulletCapacity/2;
            if (fpsController.currentWeapon.bulletCapacity > fpsController.currentWeapon.maxBulletCapacity) {
                fpsController.currentWeapon.bulletCapacity = fpsController.currentWeapon.maxBulletCapacity;
            }
            if (_audioSource.clip != addBulletSound)
                _audioSource.clip = addBulletSound;
            _audioSource.Play();
        }

        foreach (var meshRenderer in transform.GetComponentsInChildren<MeshRenderer>()) {
            meshRenderer.enabled = false;
        }
        Destroy(gameObject,3f);
    }

}