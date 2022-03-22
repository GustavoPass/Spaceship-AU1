using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
//using System.Collections;
using System.Collections.Generic;

namespace SaveLoadSystem {
    public static class SaveSystem {
        public static void SaveData(GameData data) {
            BinaryFormatter format = new BinaryFormatter();
            string path = Application.persistentDataPath + "/data.svd";
            FileStream file = new FileStream(path, FileMode.Create);
            format.Serialize(file, data);
            file.Close();
        }

        public static GameData LoadData() {
            string path = Application.persistentDataPath + "/data.svd";
            if (File.Exists(path)) {
                try {
                    BinaryFormatter format = new BinaryFormatter();
                    FileStream file = new FileStream(path, FileMode.Open);
                    GameData data = format.Deserialize(file) as GameData;
                    file.Close();
                    return data;
                } catch {
                    return new GameData();
                }
            } else {
                return new GameData();
            }
        }
    }


    [System.Serializable]
    public sealed class GameData {

        public int levelPlayer;
        public int currentXP;

        public int statusPointsRemain;
        public int healthPoints;
        public int damagePoints;
        public int reloadPoints;
        public int velocityPoints;

        public Dictionary<string, int> levelScore;

        public GameData() {
            this.levelPlayer = 1;
            this.currentXP = 0;

            this.statusPointsRemain = 0;
            this.healthPoints = 0;
            this.damagePoints = 0;
            this.reloadPoints = 0;
            this.velocityPoints = 0;

            levelScore = new Dictionary<string, int>();
        }

    }

}