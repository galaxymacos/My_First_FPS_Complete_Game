using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearDelay : MonoBehaviour {

    [SerializeField] private float disappearTime = 0.5f;

    private float currentTime = 0f;
	// Use this for initialization
	void Start () {
        currentTime = disappearTime;
    }
	
	// Update is called once per frame
	void Update () {
        currentTime -= Time.deltaTime;
        if (currentTime <= 0f) {
            Destroy(gameObject);
        }
    }
}
