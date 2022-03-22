//using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public sealed class PoolingEnemy : BasePooling, ILoadObject{

    public List<EnemyBase> enemyPool;

    private void Start() {
        if (LoadManager.instance == null) {
            InstantiateObjects();
            return;
        }

        SendToLoad();
    }
    

    public EnemyBase GetEnemy() {
        return GetPoolObj<EnemyBase>(enemyPool);
    }

    public void InstantiateObjects() {
        AddToPooling<EnemyBase>(enemyPool);
    }

    public void SendToLoad() {
        LoadManager.instance.AddObjectToLoad(this);
    }

    public int GetSize() {
        return sizePool;
    }
}
