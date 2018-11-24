using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour {

    [SerializeField] Collider rootCollider;
    [SerializeField] Collider headCollider;
	// Use this for initialization
	void Start () {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach(Collider childCollider in colliders){
            if (childCollider != rootCollider&&childCollider!=headCollider)
            {
                childCollider.enabled = false;
            }
        }
	}

    public void EnableRagdoll()
    {

        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider childCollider in colliders)
        {
            if (childCollider != rootCollider)
            {
                childCollider.GetComponent<Rigidbody>().isKinematic = false;
//                childCollider.GetComponent<Rigidbody>().velocity = Vector3.zero;
//                childCollider.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
                childCollider.enabled = true;
            }
        }
    }
}
