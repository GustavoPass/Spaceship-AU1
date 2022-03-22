//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class SoundLevelManager : MonoBehaviour{

    public static SoundLevelManager instance { get; private set; }

    private AudioSource aud;
    public AudioClip xpSound;
    public AudioClip explosionSound;
    public AudioClip contactExplosion;

    private void Start() {
        instance = this;
        aud = GetComponent<AudioSource>();
    }
    
    public void PlayXP() {
        aud.PlayOneShot(xpSound, 0.15f);
    }

    public void PlayExplosion() {
        aud.PlayOneShot(explosionSound, 0.4f);
    }

}
