//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public sealed class SpaceshipMenuRotation : MonoBehaviour {

        private Transform trans;

        void Awake() {
            trans = GetComponent<Transform>();
        }


        void Update() {
            trans.Rotate(Vector3.up * Time.deltaTime * 50f, Space.World);
        }
    }
}