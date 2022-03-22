//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class LoadManager : MonoBehaviour{

    public static LoadManager instance { get; private set; }

    public List<ILoadObject> ObjectToInstantiate;

    [SerializeField] private Slider loadBar;

    public delegate void LoadGame();
    public event LoadGame OnLoadCompleted;


    private void Awake() {
        if (instance != null) Destroy(this);
        else instance = this;
        ObjectToInstantiate = new List<ILoadObject>();

        loadBar.gameObject.SetActive(false);
    }

    public void StartLoadObjects() {
        //Começa a coroutine somente se tiver objeto para carregar
        if (ObjectToInstantiate.Count > 0)
            StartCoroutine(LoadListOfObjectToPool());
        else
            GameManager.instance.RemoveLoadingScene();
    }

    public void AddObjectToLoad(ILoadObject obj) {
        ObjectToInstantiate.Add(obj);
        loadBar.maxValue += obj.GetSize();
    }

    private IEnumerator LoadListOfObjectToPool() {
        loadBar.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.25f);

        for (int i = 0; i < ObjectToInstantiate.Count; i++) {
            yield return new WaitForSeconds(0.25f);
            ObjectToInstantiate[i].InstantiateObjects();
            loadBar.value += ObjectToInstantiate[i].GetSize();
        }

        yield return new WaitForSeconds(0.25f);

        OnLoadCompleted?.Invoke();
        GameManager.instance.RemoveLoadingScene();

    }

}
