//using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public sealed class PoolingTiroEnemy : BasePooling, ILoadObject {

    public static PoolingTiroEnemy instance { get; private set; }
    public List<TiroEnemy> tiroPool;

    private void Start() {
        instance = this;

        if (LoadManager.instance == null) {
            InstantiateObjects();
            return;
        }

        SendToLoad();
    }

    public int GetSize() {
        return sizePool;
    }

    public void SendToLoad() {
        LoadManager.instance.AddObjectToLoad(this);
    }

    public void InstantiateObjects() {
        AddToPooling<TiroEnemy>(tiroPool);
    }

    public TiroEnemy GetTiro() {
        return GetPoolObj<TiroEnemy>(tiroPool);
    }

}
