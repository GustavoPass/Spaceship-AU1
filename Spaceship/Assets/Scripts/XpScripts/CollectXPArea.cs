//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class CollectXPArea : MonoBehaviour{

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("XP")) {
            other.GetComponent<XPfragment>().CollectXP();
        }
    }
}
