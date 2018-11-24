using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyTarget : MonoBehaviour {
    [SerializeField] private GameObject flameEffect;
    [SerializeField] private GameObject dustExplosionEffect;
    [SerializeField] private AudioClip sandExplosion;
    [SerializeField] private AudioClip zombieDie;
    private AudioSource _audioSource;
    [SerializeField] private float hp = 200f;

    void Start() {
        _audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update() {
        if (hp <= 0) {
            Die();
        }
    }

    public void TakeDamage(float damage) {
        _audioSource.Play();
        hp -= damage;
        if (hp <= 0)
            Die();
    }

    public void Die() {
        
        if (gameObject.CompareTag("Weakness")) {
            GetComponent<WeaknessBreak>().BreakWeakness();

            Destroy(gameObject);
            Instantiate(dustExplosionEffect, transform.position, Quaternion.identity);
            Instantiate(flameEffect, transform.position + new Vector3(0, -1, 0), Quaternion.identity);    // To put the fire on the ground
        }
        else {
            _audioSource.clip = zombieDie;
            _audioSource.loop = false;

            _audioSource.Play();
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

            GetComponent<Rigidbody>().velocity = Physics.gravity;

            GetComponent<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<EnemyBehaviour>().enabled = false;
            GetComponent<AnimationController>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = false;

            Ragdoll ragdoll = GetComponent<Ragdoll>();
            ragdoll.EnableRagdoll();
            
        Destroy(gameObject,5);

        }
        

    }
}