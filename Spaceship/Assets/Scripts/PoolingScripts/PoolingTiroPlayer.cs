//using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public sealed class PoolingTiroPlayer : BasePooling, ILoadObject{

    public List<TiroPlayer> tiroPool;

    private void Start(){

        if (LoadManager.instance == null) {
            InstantiateObjects();
            return;
        }

        SendToLoad();
    }

    public TiroPlayer GetTiro() {
        return GetPoolObj<TiroPlayer>(tiroPool);
    }

    public void InstantiateObjects() {
        AddToPooling<TiroPlayer>(tiroPool);
    }

    public void SendToLoad() {
        LoadManager.instance.AddObjectToLoad(this);
    }

    public int GetSize() {
        return sizePool;
    }
}