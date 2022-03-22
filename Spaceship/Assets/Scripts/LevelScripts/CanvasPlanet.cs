//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;

public sealed class CanvasPlanet : MonoBehaviour{

    public delegate void openUI(bool b);
    public event openUI OnShowUI;


    [Header("Canvas level")]
    [SerializeField] private GameObject canvasLevel;
    [SerializeField] private TextMeshProUGUI nameLevel;
    [SerializeField] private TextMeshProUGUI score;

    [Header("Map Play")]
    [SerializeField] private GameObject playMap;
    [SerializeField] private TextMeshProUGUI nameLevel2;
    [SerializeField] private TextMeshProUGUI score2;
    [SerializeField] private TextMeshProUGUI difficultText;

    private int difficultLevel;

    private Transform trans;
    private Vector3 offset;

    private string levelName;

    void Start(){
        trans = canvasLevel.GetComponent<Transform>();
        HideCanvas();
        offset = Vector3.up * -6;
    }

    public void ShowCanvas(string name, int sc, Vector3 pos) {
        canvasLevel.SetActive(true);
        nameLevel.text = name;
        nameLevel2.text = name;
        score.text = "score: " + sc;
        score2.text = "score: " + sc;
        trans.position = pos - offset;
    }

    public void HideCanvas() {
        canvasLevel.SetActive(false);
    }

    public void ShowPlayMap(string lvlName) {
        playMap.SetActive(true);
        levelName = lvlName;
        OnShowUI(false);

        difficultLevel = 1;
        ChangeDifficult(0);
    }

    public void CancelPlayMap() {
        playMap.SetActive(false);
        OnShowUI(true);
    }

    public void PlayMap() {
        GameManager.instance.SetDifficult(difficultLevel);

        string[] scenesToLoad = { levelName, Constantes.sceneCanvasLevel, Constantes.scenePosGame };

        GameManager.instance.levelName = levelName;
        GameManager.instance.LoadScenes(scenesToLoad);
    }

    public void ChangeDifficult(int n) {
        difficultLevel += n;
        if (difficultLevel < 1) difficultLevel = 1;
        else if (difficultLevel > 3) difficultLevel = 3;

        difficultText.text = GameManager.instance.GetDifficultName(difficultLevel);
    }


}
