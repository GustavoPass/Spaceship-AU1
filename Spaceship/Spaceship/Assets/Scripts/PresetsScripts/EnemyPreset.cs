//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "Presets/Enemy")]
public sealed class EnemyPreset : ScriptableObject {
    public float velocity;
    public int life;
    public float reloadTime;

    public int scoreOnKill;
}
