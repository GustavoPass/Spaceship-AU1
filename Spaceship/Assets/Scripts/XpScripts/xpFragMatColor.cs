//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class xpFragMatColor : MonoBehaviour{

    [Header("MAT")]
    public Material xpMat;
    public Gradient grad;

    [Header("Values")]
    public float cycleTime;
    private float currentTime;

    private void Update(){
        if (currentTime >= 1) currentTime = 0;
        currentTime += Time.deltaTime * cycleTime;

        xpMat.SetColor("_EmissionColor", grad.Evaluate(currentTime) * 1.2f);
    }
}
