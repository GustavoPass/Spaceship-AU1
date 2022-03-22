//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class HabilidadeUnica1 : MonoBehaviour{

    private Transform trans;
    public float velocity;

    void Start(){
        trans = GetComponent<Transform>();
        velocity *= 100;
    }

    
    void Update(){
        trans.Rotate(Vector3.up * Time.deltaTime * velocity);
    }
}
