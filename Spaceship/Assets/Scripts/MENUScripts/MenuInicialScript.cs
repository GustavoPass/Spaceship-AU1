//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public sealed class MenuInicialScript : MonoBehaviour{

    [Header("UI elements")]
    [SerializeField] private GameObject menuInicialUI;
    [SerializeField] private GameObject startUI;
    [SerializeField] private GameObject spaceShipPointsUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject playMapUI;
    [SerializeField] private GameObject Guide;

    [Header("Scene elements")]
    [SerializeField] private GameObject menuInicialScene;
    [SerializeField] private GameObject mapaSelectScene;
    [SerializeField] private GameObject spaceShipPointsScene;
    [SerializeField] private GameObject AdvisePoints;

    public static bool startWithMapSelect;

    private void Awake(){
        if (startWithMapSelect) ShowMapaSelect();
        else ShowMenuInicial();
    }

    private void Start() {
        startWithMapSelect = true;
        if (GameManager.instance.pointsToAddStatus > 0) AdvisePoints.SetActive(true);
        else AdvisePoints.SetActive(false);
    }


    public void ShowMenuInicial() {
        menuInicialUI.SetActive(true);
        menuInicialScene.SetActive(true);
        startUI.SetActive(false);
        mapaSelectScene.SetActive(false);
        spaceShipPointsScene.SetActive(false);
        spaceShipPointsUI.SetActive(false);
        optionsUI.SetActive(false);
        playMapUI.SetActive(false);
        Guide.SetActive(false);
    }
    public void ShowMapaSelect() {
        startUI.SetActive(true);
        mapaSelectScene.SetActive(true);
        menuInicialUI.SetActive(false);
        menuInicialScene.SetActive(false);
        spaceShipPointsScene.SetActive(false);
        spaceShipPointsUI.SetActive(false);
        optionsUI.SetActive(false);
        playMapUI.SetActive(false);
        Guide.SetActive(false);
    }

    public void ShowOptions() {
        optionsUI.SetActive(true);
        startUI.SetActive(false);
        mapaSelectScene.SetActive(false);
        menuInicialUI.SetActive(false);
        menuInicialScene.SetActive(false);
        spaceShipPointsScene.SetActive(false);
        spaceShipPointsUI.SetActive(false);
        playMapUI.SetActive(false);
        Guide.SetActive(false);
    }

    public void ShowSpaceShipPoints() {
        startUI.SetActive(true);
        spaceShipPointsScene.SetActive(true);
        spaceShipPointsUI.SetActive(true);
        menuInicialUI.SetActive(false);
        menuInicialScene.SetActive(false);
        mapaSelectScene.SetActive(false);
        optionsUI.SetActive(false);
        playMapUI.SetActive(false);
        Guide.SetActive(false);
    }

    public void ShowGuide() {
        startUI.SetActive(false);
        spaceShipPointsScene.SetActive(false);
        spaceShipPointsUI.SetActive(false);
        menuInicialUI.SetActive(false);
        menuInicialScene.SetActive(false);
        mapaSelectScene.SetActive(false);
        optionsUI.SetActive(false);
        playMapUI.SetActive(false);
        Guide.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }


}
