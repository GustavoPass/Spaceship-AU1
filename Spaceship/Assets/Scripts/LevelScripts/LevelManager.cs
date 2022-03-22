//using System;
//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class LevelManager : MonoBehaviour{

    public static LevelManager instance { get; private set; }

    public delegate void EndGame();
    public event EndGame OnDefeat;
    public event EndGame OnVictory;

    public float pontuacao { get; private set; }
    public int pontuacaoGanha;
    public int xpGanha;

    public int difficultMultiply { get; private set; }

    void Awake(){
        instance = this;
        if (GameManager.instance == null) {
            var go = new GameObject();
            go.AddComponent<GameManager>();
        }
        difficultMultiply = GameManager.instance.GetDifficultMultiply();
    }

    private void Update() {
        pontuacao = Mathf.MoveTowards(pontuacao, pontuacaoGanha, Time.deltaTime * 8000 * difficultMultiply);
    }

    public void AddXP(int xp) {
        xpGanha += xp;
    }

    public void Pontuar(int pontos) {
        pontuacaoGanha += pontos;
    }

    public int GetScore() {
        return pontuacaoGanha;
    }

    public void EndGameDefeat() {
        OnDefeat?.Invoke();
    }

    public void EndGameVictory() {
        OnVictory?.Invoke();
    }

}
