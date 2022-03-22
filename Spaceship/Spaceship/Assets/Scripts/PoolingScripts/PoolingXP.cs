//using System.Collections;
using System.Collections.Generic;
//using UnityEngine;

public sealed class PoolingXP : BasePooling, ILoadObject{

    public static PoolingXP instance { get; private set; }
    public List<XPfragment> xpPool;

    private void Start() {
        instance = this;

        if(LoadManager.instance == null) {
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
        AddToPooling<XPfragment>(xpPool);
    }

    public XPfragment GetXP() {
        return GetPoolObj<XPfragment>(xpPool);
    }

}
