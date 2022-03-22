//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class XPfragment : MonoBehaviour{

    private Transform player;
    private Transform trans;

    private bool isCollected;

    private void Awake() {
        trans = GetComponent<Transform>();
        isCollected = false;
    }

    private void Start() {
        player = PlayerShipController.instance.GetTransform();
        gameObject.SetActive(false);

    }

    private void Update() {
        trans.position = Vector3.MoveTowards(trans.position, player.position, Time.deltaTime * Constantes.XPvelocity / 2);

        if (!isCollected) return;
        trans.position = Vector3.SlerpUnclamped(trans.position, player.position, Time.deltaTime * Constantes.XPvelocity);
    }

    public void SpawnFragment(Transform enemyPos) {
        gameObject.SetActive(true);
        isCollected = false;
        trans.position = enemyPos.position + randomSpawn();
        LevelManager.instance.OnVictory += CollectXP;
    }

    private Vector3 randomSpawn() {
        return (Vector3.right * Random.Range(-2.0f, 2.0f)) + (Vector3.forward * Random.Range(-2.0f, 2.0f));
    }

    public void CollectXP() {
        isCollected = true;
        LevelManager.instance.OnVictory -= CollectXP;
    }

    public int GetXP() {
        gameObject.SetActive(false);
        return Random.Range(Constantes.minXP, Constantes.maxXP) * LevelManager.instance.difficultMultiply;
    }

}
