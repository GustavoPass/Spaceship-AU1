//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class TiroPlayerParticle : MonoBehaviour{

    private Transform trans;
    private ParticleSystem particle;
    
    void Awake(){
        trans = GetComponent<Transform>();
        particle = GetComponent<ParticleSystem>();
    }

    
    public void Explode(Transform pos) {
        ToggleActive(true);
        trans.position = pos.position;
        particle.Play();
    }

    private void ToggleActive(bool b) {
        gameObject.SetActive(b);
    }


}
