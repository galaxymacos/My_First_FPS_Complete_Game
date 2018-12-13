using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class PauseScreenBehavior : MonoBehaviour {

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField]private AudioMixer audioMixer;

    
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeMusicVolume(float vol)
    {
        audioMixer.SetFloat("MusicVolume", vol);
    }

    public void ChangeSfxVolume(float vol)
    {
        audioMixer.SetFloat("SfxVolume", vol);
    }
}
