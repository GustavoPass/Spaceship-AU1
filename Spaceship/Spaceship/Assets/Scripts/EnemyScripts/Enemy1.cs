//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class Enemy1 : EnemyBase{

    private void Update() {
        RedMaterial();

        if (reloadTime > 0) {
            reloadTime -= Time.deltaTime;
        } else {
            BulletShoot();
        }
    }

    public override void BulletShoot() {
        base.BulletShoot();

        for (int i = 0; i < muzzles.Length; i++) {
            var tiro = PoolingTiroEnemy.instance.GetTiro();
            tiro.Shoot(muzzles[i], muzzles[i].right);
            tiro.RotateDirection(muzzles[i].right);
        }

    }



}
