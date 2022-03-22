//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using SunCheat;

public sealed class LevelSelect : MonoBehaviour{

    [SerializeField] private CanvasPlanet levelinfo;

    private Transform trans;

    [SerializeField] private string planetName;
    [SerializeField] private int scoreRecord;
    [SerializeField] private string sceneName;
    [SerializeField] private float posYoffset;

    private bool canInteract;

    private void Start() {
        trans = GetComponent<Transform>();

        scoreRecord = GameManager.instance.GetLevelHighScore(sceneName);

        SunUnlock.OnClickSunToReset += ResetScore;
        levelinfo.OnShowUI += ChangeInteract;

        canInteract = true;
    }

    private void OnEnable() {
        canInteract = true;
    }

    private void ChangeInteract(bool b) {
        canInteract = b;
    }

    private void OnMouseDown() {
        if(canInteract) levelinfo.ShowPlayMap(sceneName);

        /*
        string[] scenesToLoad = { sceneName, Constantes.sceneCanvasLevel, Constantes.scenePosGame };

        GameManager.instance.levelName = sceneName;
        GameManager.instance.LoadScenes(scenesToLoad);
        */
    }

    private void OnDestroy() {
        SunUnlock.OnClickSunToReset -= ResetScore;
        levelinfo.OnShowUI -= ChangeInteract;
    }

    private void ResetScore() {
        scoreRecord = GameManager.instance.GetLevelHighScore(sceneName);
    }

    private void OnMouseOver() {
        if (canInteract)
            levelinfo.ShowCanvas(planetName, scoreRecord, trans.position + Vector3.up * posYoffset);
    }

    private void OnMouseExit() {
        levelinfo.HideCanvas();
    }

}
