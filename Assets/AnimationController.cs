using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimationController : MonoBehaviour {
    private Animator anim;

    private NavMeshAgent nma;
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        nma = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 movementVector = transform.InverseTransformDirection(nma.velocity).normalized;
        anim.SetFloat("H",movementVector.x);
        anim.SetFloat("V",movementVector.z);

    }
}
