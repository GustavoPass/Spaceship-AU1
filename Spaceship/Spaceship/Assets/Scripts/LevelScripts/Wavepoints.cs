//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class Wavepoints : MonoBehaviour{

    public Transform[] points;

    void Start(){
        Transform t = GetComponent<Transform>();
        points = new Transform[transform.childCount];
        for (int i = 0; i < points.Length; i++) {
            points[i] = t.GetChild(i).GetComponent<Transform>();
        }
    }


}
