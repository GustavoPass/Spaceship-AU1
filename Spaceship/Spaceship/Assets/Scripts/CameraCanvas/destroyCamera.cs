//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace cameraManager {
    public sealed class destroyCamera : MonoBehaviour {
        private void Start() {
            Destroy(gameObject);
        }

    }
}
