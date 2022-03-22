//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

namespace Map {
    public sealed class MapMove : MonoBehaviour {

        [SerializeField] private Transform background;
        [SerializeField] private Transform planets;
        [SerializeField] private float vel;

        [SerializeField] private float panBorderThickness;
        [SerializeField] private float mapLimit;

        private LineRenderer tragetoria;

        private void Awake() {
            tragetoria = planets.GetComponent<LineRenderer>();
            tragetoria.positionCount = planets.childCount;
            planets.position = Vector3.zero;

            LineUpdate();
        }

        private void Update() {

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width - panBorderThickness) {
                MoveLeft();
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= panBorderThickness) {
                MoveRight();
            }

        }

        private void MoveRight() {
            if (planets.transform.position.x >= 0) return;

            planets.Translate(Vector3.right * vel * Time.deltaTime);
            background.Translate(Vector3.right * vel / 10 * Time.deltaTime);

            LineUpdate();
        }
        private void MoveLeft() {
            if (planets.transform.position.x <= mapLimit) return;

            planets.Translate(Vector3.left * vel * Time.deltaTime);
            background.Translate(Vector3.left * vel / 10 * Time.deltaTime);

            LineUpdate();
        }

        private void LineUpdate() {
            for (int i = 0; i < planets.childCount; i++) {
                tragetoria.SetPosition(i, planets.GetChild(i).position);
            }
        }

    }
}
