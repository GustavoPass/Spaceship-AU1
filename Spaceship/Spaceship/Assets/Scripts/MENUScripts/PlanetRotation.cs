//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public sealed class PlanetRotation : MonoBehaviour {

        private Transform trans;
        [SerializeField] private float vel;

        void Awake() {
            trans = GetComponent<Transform>();
        }

        void Update() {
            trans.Rotate(trans.up * Time.deltaTime * vel, Space.World);
        }

    }
}