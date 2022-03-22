//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Background {
    public sealed class BackgroundMove : MonoBehaviour {

        [SerializeField] private Transform[] bg;

        [SerializeField] private float vel;
        [SerializeField] private float bgYreset;

        private void Start() {
            if (LoadManager.instance == null) return;

            enabled = false;
            LoadManager.instance.OnLoadCompleted += LoadComplete;
        }

        private void LoadComplete() {
            enabled = true;
            LoadManager.instance.OnLoadCompleted -= LoadComplete;
        }

        void Update() {
            for (int i = 0; i < bg.Length; i++) {
                bg[i].Translate(Vector3.down * vel * Time.deltaTime, Space.World);
                if (bg[i].localPosition.y < -100) {
                    bg[i].localPosition = Vector3.up * bgYreset;
                }
            }
        }
    }
}