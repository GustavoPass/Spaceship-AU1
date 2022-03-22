//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class EnemyBase : MonoBehaviour, IEndGame{

    public EnemyPreset preset;

    [HideInInspector] public Transform trans;
    private Rigidbody rb;
    private Material mat;
    [HideInInspector] public Transform[] muzzles;

    private int life;
    private float vel;
    private Vector3 rotInitial;

    [HideInInspector] public float reloadTime;

    private float materialTime;

    private void Awake() {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();

        muzzles = new Transform[trans.childCount];

        for (int i = 0; i < trans.childCount; i++) {
            muzzles[i] = trans.GetChild(i).GetComponent<Transform>();
        }

        var rend = GetComponent<Renderer>();
        mat = new Material(rend.material);
        rend.material = mat;
    }

    private void Start() {
        SetDefaultValues();
        rotInitial = trans.eulerAngles;

        ToggleActive(false);
    }

    private void SetDefaultValues() {
        life = preset.life * LevelManager.instance.difficultMultiply;
        vel = preset.velocity;
        reloadTime = preset.reloadTime;
    }


    public virtual void BulletShoot() {
        reloadTime = preset.reloadTime;
    }

    public void Spawn(Transform point) {
        ToggleActive(true);
        trans.position = point.position;
        rb.velocity = point.up * -vel;
        trans.rotation = Quaternion.Euler(new Vector3(point.eulerAngles.z, rotInitial.y, rotInitial.z));
    }

    public void ReduceSpeed() {
        rb.velocity = Vector3.Lerp(rb.velocity, Vector3.up * -vel / 20, Time.deltaTime * 3) ;
    }

    public void Defeat() {
        PoolingExplosaoInimigo.instance.GetParticle().Explode(trans);
        DecreaseEnemyNumber();
        ToggleActive(false);
    }

    private void DecreaseEnemyNumber() {
        WaveController.instance.ChangeEnemyCount(-1);
    }

    public void Victory() { }

    public void Damage(int dano) {
        life -= dano;
        materialTime = Constantes.materialDamageTimeReset;
        mat.SetColor("_EmissionColor", Color.white);

        if (life <= 0) {
            PoolingExplosaoInimigo.instance.GetParticle().Explode(trans);
            PoolingXP.instance.GetXP().SpawnFragment(trans);
            LevelManager.instance.Pontuar(preset.scoreOnKill * LevelManager.instance.difficultMultiply);
            SoundLevelManager.instance.PlayExplosion();
            DecreaseEnemyNumber();
            ToggleActive(false);
        }
    }

    public void RedMaterial() {
        if (materialTime > 0) materialTime -= Time.deltaTime;
        else mat.SetColor("_EmissionColor", Color.red * 1.2f);
    }

    private void ToggleActive(bool b) {
        gameObject.SetActive(b);
        if (b) {
            SetDefaultValues();
            LevelManager.instance.OnDefeat += Defeat;
        } else {
            LevelManager.instance.OnDefeat -= Defeat;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("StopEnemy")) {
            ToggleActive(false);
            DecreaseEnemyNumber(); // insta kill
            return;
        }

        if (other.CompareTag("Player")) {
            int dano = 2 * LevelManager.instance.difficultMultiply;
            if (dano < 15) dano = 15;

            other.GetComponent<PlayerShipController>().Damage(dano);
            PoolingExplosaoInimigo.instance.GetParticle().Explode(trans);
            SoundLevelManager.instance.PlayExplosion();
            DecreaseEnemyNumber(); // insta kill

            ToggleActive(false);
        }

    }


}
