//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using SaveLoadSystem;

namespace SunCheat {
    public sealed class SunUnlock : MonoBehaviour {

        private int clicks;

        public delegate void ResetScores();
        public static ResetScores OnClickSunToReset;

        private void OnEnable() {
            clicks = 0;
        }

        private void OnMouseOver() {
            if (Input.GetKey(KeyCode.S)) {
                if (Input.GetMouseButtonDown(0)) {
                    clicks += 1;
                    if (clicks >= 8) {
                        GetLevel30();
                        clicks = 0;
                        gameObject.SetActive(false);
                    }
                }
            }

            if (Input.GetKey(KeyCode.N)) {
                if (Input.GetMouseButtonDown(0)) {
                    clicks += 1;
                    if (clicks >= 11) {
                        ResetData();
                        clicks = 0;
                        gameObject.SetActive(false);
                    }
                }
            }

        }

        private void OnMouseExit() {
            clicks = 0;
        }

        private void GetLevel30() {
            GameData unlock = SaveSystem.LoadData();

            unlock.levelPlayer = Constantes.levelMax;
            unlock.statusPointsRemain = (Constantes.levelMax - 1) * Constantes.pointsLevelUp;
            unlock.currentXP = 0;

            unlock.healthPoints = 0;
            unlock.damagePoints = 0;
            unlock.reloadPoints = 0;
            unlock.velocityPoints = 0;


            GameManager.instance.status.healthPoints = 0;
            GameManager.instance.status.damagePoints = 0;
            GameManager.instance.status.reloadPoints = 0;
            GameManager.instance.status.velocityPoints = 0;

            GameManager.instance.currentXpLevel = 0;
            GameManager.instance.levelPlayer = unlock.levelPlayer;
            GameManager.instance.pointsToAddStatus = unlock.statusPointsRemain;

            SaveSystem.SaveData(unlock);
            Debug.Log("unlock lvl 30");

        }

        private void ResetData() {
            GameData nd = new GameData();
            GameManager.instance.status.healthPoints = 0;
            GameManager.instance.status.damagePoints = 0;
            GameManager.instance.status.reloadPoints = 0;
            GameManager.instance.status.velocityPoints = 0;

            GameManager.instance.currentXpLevel = 0;
            GameManager.instance.levelPlayer = 1;
            GameManager.instance.pointsToAddStatus = 0;

            GameManager.instance.ResetScore();

            SaveSystem.SaveData(nd);
            Debug.Log("reset data");
            OnClickSunToReset();
        }

    }
}
