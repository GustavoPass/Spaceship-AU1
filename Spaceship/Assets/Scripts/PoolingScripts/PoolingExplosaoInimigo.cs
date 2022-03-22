//using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public sealed class PoolingExplosaoInimigo : BasePooling, ILoadObject{

    public static PoolingExplosaoInimigo instance { get; private set; }
    public List<TiroPlayerParticle> pool;

    private void Start() {
        instance = this;

        if (LoadManager.instance == null) {
            InstantiateObjects();
            return;
        }

        SendToLoad();
    }

    public TiroPlayerParticle GetParticle() {
        return GetPoolObj<TiroPlayerParticle>(pool);
    }

    public void InstantiateObjects() {
        AddToPooling<TiroPlayerParticle>(pool);
    }

    public void SendToLoad() {
        LoadManager.instance.AddObjectToLoad(this);
    }

    public int GetSize() {
        return sizePool;
    }
}
