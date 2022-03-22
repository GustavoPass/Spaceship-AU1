//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace cameraManager {
    public sealed class cameraRef : MonoBehaviour {

        public Camera cam;
        public static cameraRef instance { get; private set; }

        private void Awake() {
            instance = this;
        }

    }
}
