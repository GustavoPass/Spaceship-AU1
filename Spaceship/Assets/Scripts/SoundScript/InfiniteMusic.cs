//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public sealed class InfiniteMusic : MonoBehaviour{

    private AudioSource aud;

    [SerializeField] private AudioClip[] musicas;
    [SerializeField] private int index;


    private void Awake(){
        aud = GetComponent<AudioSource>();
        index = 0;
        PlayMusic();
    }

    private void Update(){
        if (!aud.isPlaying) {
            PlayMusic();
        }
    }

    private void PlayMusic() {
        aud.clip = musicas[index];
        aud.Play();

        index = (index + 1) % musicas.Length;
    }



}
