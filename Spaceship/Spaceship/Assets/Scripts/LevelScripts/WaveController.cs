using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class WaveController : MonoBehaviour, IEndGame{

    public static WaveController instance { get; private set; }

    [SerializeField] private WaveSet[] waves;
    private float cooldownSpawn;
    private int currentWave;
    private bool endGame;
    private bool isSpawning;

    private int InimigosAtivos;

    private bool pauseSpawn;

    [SerializeField] private bool infiniteMode;

    private void Start(){
        isSpawning = false;
        endGame = false;
        instance = this;
        cooldownSpawn = 0f;
        InimigosAtivos = 0;

        LevelManager.instance.OnDefeat += Defeat;
        LevelManager.instance.OnVictory += Victory;

        if (LoadManager.instance == null) return;

        LoadManager.instance.OnLoadCompleted += CompletedLoad;
        enabled = false;
        pauseSpawn = true;
    }

    private void OnDestroy() {
        LevelManager.instance.OnDefeat -= Defeat;
    }

    private void CompletedLoad() {
        enabled = true;
        StartCoroutine(StartSpawn());
        LoadManager.instance.OnLoadCompleted -= CompletedLoad;
    }

    private void Update(){

        if (endGame) {
            if (InimigosAtivos <= 0) LevelManager.instance.EndGameVictory();
            return;
        }

        if (isSpawning || pauseSpawn) return;
        
        if (cooldownSpawn > 0) {
            cooldownSpawn -= Time.deltaTime;

            if (InimigosAtivos <= 0) cooldownSpawn = 0;

        } else {
            SpawnEnemys();
        }
    }

    public void Defeat() {
        cooldownSpawn = 100;
        isSpawning = true;
        StopCoroutine(SpawnTimer());
        StopCoroutine(SpawnInverseTimer());
        enabled = false;
    }

    public void Victory() {
        LevelManager.instance.OnVictory -= Victory;
        enabled = false;
    }

    private void SpawnEnemys() {

        cooldownSpawn = waves[currentWave].timeNextWave;

        //Spawn por tempo
        if (waves[currentWave].timeBetweenEnemy > 0) {
            isSpawning = true;
            if (waves[currentWave].inverseSpawn) StartCoroutine(SpawnInverseTimer());
            else StartCoroutine(SpawnTimer());
            return;
        }

        //Spawn instantaneo
        for (int i = 0; i < waves[currentWave].spawnPoints.points.Length; i++) {
            waves[currentWave].enemyPool.GetEnemy().Spawn(waves[currentWave].spawnPoints.points[i]);
            InimigosAtivos += 1;
        }
        WaveCount();
    }


    private IEnumerator SpawnTimer() {
        for (int i = 0; i < waves[currentWave].spawnPoints.points.Length; i++) {
            yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemy);
            waves[currentWave].enemyPool.GetEnemy().Spawn(waves[currentWave].spawnPoints.points[i]);
            InimigosAtivos += 1;
        }
        WaveCount();
        isSpawning = false;
    }


    private IEnumerator SpawnInverseTimer() {
        for (int i = waves[currentWave].spawnPoints.points.Length - 1; i >= 0 ; i--) {
            yield return new WaitForSeconds(waves[currentWave].timeBetweenEnemy);
            waves[currentWave].enemyPool.GetEnemy().Spawn(waves[currentWave].spawnPoints.points[i]);
            InimigosAtivos += 1;
        }
        WaveCount();
        isSpawning = false;
    }

    private IEnumerator StartSpawn() {
        yield return new WaitForSeconds(2f);
        pauseSpawn = false;
    }


    private void WaveCount() {
        currentWave += 1;

        if (currentWave >= waves.Length) {
            if (infiniteMode) currentWave = 0;
            else endGame = true;
        }
    }

    public void ChangeEnemyCount(int num) {
        InimigosAtivos += num;
    }

}

[System.Serializable]
public struct WaveSet {
    public PoolingEnemy enemyPool;
    public Wavepoints spawnPoints;
    public float timeBetweenEnemy;
    public float timeNextWave;
    public bool inverseSpawn;
}