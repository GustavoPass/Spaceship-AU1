//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BasePooling : MonoBehaviour{

    public GameObject objPrefab;
    public int sizePool;
    private int poolIndex;

    private Transform trans => GetComponent<Transform>();

    protected void AddToPooling<T>(List<T> lista) {

        for (int i = 0; i < sizePool; i++) {
            var go = Instantiate(objPrefab, trans);
            lista.Add(go.GetComponent<T>());
        }
    }

    protected T GetPoolObj<T>(List<T> lista){
        T t = lista[poolIndex];
        poolIndex = (poolIndex + 1) % lista.Count;
        return t;
    }

    /*
    public void SendPool(PoolingManager.PoolMaster pool){
        var pool = GetComponentInParent<PoolingManager>();
        pool.pl.Add(pool);
    }
    */

}