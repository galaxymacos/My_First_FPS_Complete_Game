using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInformation : MonoBehaviour {
    private int weaknessNumber = 5; // TODO change back to 5

    [SerializeField] private GameObject explosionPoint;

    [SerializeField] private float explosionForce = 1000f;

    [SerializeField] private float explosionRadius = 100f;
    [SerializeField] internal AudioSource backgroundMusic;
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField] private SpawnPoint SpawnPoint;
    [SerializeField] private GameObject VictoryBoard;
    
    [SerializeField] private GameObject metalBarSpawner;
    [SerializeField] private GameObject metalBarAttack;
    [SerializeField] private float metalBarAttackInterval = 15f;
    [SerializeField] private AutoRotate autoRotate;
    private float metalBarCurrentTime;

    private void Start() {
        metalBarCurrentTime = metalBarAttackInterval;

    }

    private void Update() {
        metalBarCurrentTime -= Time.deltaTime;
        if (metalBarCurrentTime <= 0f) {
            metalBarCurrentTime = metalBarAttackInterval;
            InitiateMetalBarAttack();
        }
    }

    public void BreakWeakness() {
        weaknessNumber -= 1;
        if (weaknessNumber == 0) {
            BossExplosion();
        }

        SpawnPoint.spawnInterval -= 1;
        autoRotate.rotateSpeed *= 1.3f;
    }

    private void BossExplosion() {

        StartCoroutine(PlayVictoryMusic());
       
        DisableAllAnimationAndScriptWhenGameOver();    // TODO change name
        
        
        GetComponent<Animator>().enabled = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (var nearbyObject in colliders) {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null) {
                rb.isKinematic = false;
                rb.useGravity = true;
                rb.AddExplosionForce(explosionForce, explosionPoint.transform.position, explosionRadius);                
            }
        }

        StartCoroutine(LoadVictoryBoard());
    }

    IEnumerator PlayVictoryMusic() {
        // replace the gloomy background music to a happy one
        if (backgroundMusic.clip != victoryMusic) {
            backgroundMusic.clip = victoryMusic;
        }
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(2f);
        
        backgroundMusic.loop = false;
        backgroundMusic.Play();
    }

    IEnumerator LoadVictoryBoard() {
        yield return new WaitForSeconds(3f);
        VictoryBoard.SetActive(true);
        
    }
    
    private void DisableAllAnimationAndScriptWhenGameOver() {
        SpawnPoint.enabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies) {
            Destroy(enemy);
        }
    }

    public void InitiateMetalBarAttack() {
        Instantiate(metalBarAttack, metalBarSpawner.transform.position, Quaternion.identity);
    }
}