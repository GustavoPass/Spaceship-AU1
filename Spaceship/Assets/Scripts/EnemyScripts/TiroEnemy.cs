//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class TiroEnemy : MonoBehaviour{

    private Transform trans;
    private Rigidbody rb;
    private int dano;

    private void Awake() {
        trans = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        ToggleActive(false);
    }

    private void Start() {
        dano = 10 * (LevelManager.instance.difficultMultiply / 5);
        if (dano < 10) dano = 10;
    }

    public void Shoot(Transform pos, Vector3 direction) {
        ToggleActive(true);
        trans.position = pos.position;
        rb.velocity = direction * -20;
    }

    public void RotateDirection(Vector3 direction) {
        Quaternion lootToEnemy = Quaternion.LookRotation(direction);
        Vector3 rot = lootToEnemy.eulerAngles;
        trans.rotation = Quaternion.Euler(rot + (Vector3.right * 90));
    }

    private void ToggleActive(bool b) {
        gameObject.SetActive(b);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("StopEnemy")) {
            ToggleActive(false);
            return;
        }

        if (other.CompareTag("PlayerHabilidadeUnica")) {
            ToggleActive(false);
            return;
        }

        if (other.CompareTag("Player")) {
            other.GetComponent<PlayerShipController>().Damage(dano);
            PoolingExplosionTiroParticle.instance.GetParticle().Explode(trans);
            ToggleActive(false);
            return;
        }
    }

}
