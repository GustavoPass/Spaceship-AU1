//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy5 : EnemyBase{

    
    [SerializeField] private int shootCount;
    [SerializeField] private int totalShootCount;
    private int currentShootCount;

    [SerializeField] private float shootTime;
    private float cooldownTime;
    

    private void Update() {
        RedMaterial();

        if (reloadTime > 0) {
            reloadTime -= Time.deltaTime;
            currentShootCount = shootCount;
        } else {
            BulletShoot();
        }
    }

    public override void BulletShoot() {

        if (totalShootCount <= 0) return;

        if (currentShootCount <= 0) {
            base.BulletShoot();
            totalShootCount -= 1;
        }

        if (cooldownTime <= 0) {
            Vector3 dir = -(PlayerShipController.instance.GetTransform().position - trans.position).normalized;
            dir.z = 0;

            for (int i = 0; i < muzzles.Length; i++) {
                var tiro = PoolingTiroEnemy.instance.GetTiro();
                tiro.Shoot(muzzles[i], dir);
                tiro.RotateDirection(dir);
            }
            cooldownTime = shootTime;
            currentShootCount -= 1;
        } else {
            cooldownTime -= Time.deltaTime;
        }

    }


}
