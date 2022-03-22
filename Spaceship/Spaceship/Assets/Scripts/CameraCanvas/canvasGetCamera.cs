//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace cameraManager {
    public sealed class canvasGetCamera : MonoBehaviour {

        //[SerializeField] private Canvas can;

        private void Start() {

            if (cameraRef.instance == null) return;

            Canvas can = GetComponent<Canvas>();
            can.worldCamera = cameraRef.instance.cam;
        }

    }
}
