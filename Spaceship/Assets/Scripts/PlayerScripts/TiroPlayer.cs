//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class TiroPlayer : MonoBehaviour{

    private Rigidbody rb;
    private Transform trans;
    public TiroPlayerPreset preset;

    public int damage;
    private float velocity;
    private int pierceEnemys;


    void Awake(){
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
        ToggleActive(false);
    }

    private void SetDefaultValues() {
        damage = (int)(preset.damage * GameManager.instance.status.damageMultiply);
        velocity = preset.velocity * 100;
        pierceEnemys = preset.pierceEnemys;
    }

    public void Shoot(Transform muzzle) {
        ToggleActive(true);
        SetDefaultValues();
        trans.position = muzzle.position;
        trans.rotation = muzzle.rotation;
        rb.AddForce(muzzle.right * -velocity);
    }

    public void ToggleActive(bool b) {
        gameObject.SetActive(b);
        if (b) {
            rb.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag("Enemy")) {
            other.GetComponent<EnemyBase>().Damage(damage);
            PoolingExplosionTiroParticle.instance.GetParticle().Explode(trans);
            pierceEnemys -= 1;
            if(pierceEnemys <= 0) ToggleActive(false);
            return;
        }

        if (other.CompareTag("EnemyShield")) {
            if (!preset.passShield) ToggleActive(false);
            return;
        }

        if (other.CompareTag("Finish")) {
            ToggleActive(false);
            return;
        }

    }

}
