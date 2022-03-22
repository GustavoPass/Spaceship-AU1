//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TiroPlayer", menuName = "Presets/Tiro Player")]
public sealed class TiroPlayerPreset : ScriptableObject {
    public int damage;
    public float velocity;
    public int pierceEnemys;
    public bool passShield;
}
