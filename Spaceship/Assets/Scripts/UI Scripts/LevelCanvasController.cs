//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class LevelCanvasController : MonoBehaviour{

    //public static LevelCanvasController instance { get; private set; }

    [Header("UI screens")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject gameUI;

    [Header("LifeDisplay")]
    [SerializeField] private Slider lifeBar;
    [SerializeField] private Image lifeBarImageColor;
    [SerializeField] private Gradient lifeBarColor;

    [Header("Outros")]
    public Image habilidadeSelecionada;
    public TextMeshProUGUI pontuacao;

    private bool isPaused;

    void Awake(){
        //instance = this;
        pontuacao.text = string.Format(Constantes.scoreStringFormat, 0);
        isPaused = false;
        Pause();
    }

    private void Start() {
        LevelManager.instance.OnDefeat += Defeat;
        LevelManager.instance.OnVictory += Victory;
        PlayerShipController.instance.OnDamage += LifeDisplay;
        PlayerShipController.instance.ShareSprite = HabilidadeIMG;

        if (LoadManager.instance == null) return;

        LoadManager.instance.OnLoadCompleted += CompletedLoad;
        gameUI.SetActive(false);
        enabled = false;
    }
    private void OnDestroy() {
        LevelManager.instance.OnDefeat -= Defeat;
        LevelManager.instance.OnVictory -= Victory;
        PlayerShipController.instance.OnDamage -= LifeDisplay;
    }

    private void CompletedLoad() {
        enabled = true;
        gameUI.SetActive(true);
        lifeBar.maxValue = PlayerShipController.instance.GetLife();
        lifeBar.value = lifeBar.maxValue;
        lifeBarImageColor.color = lifeBarColor.Evaluate(1);

        LoadManager.instance.OnLoadCompleted -= CompletedLoad;
    }

    private void LifeDisplay(int life) {
        lifeBar.value = life;
        lifeBarImageColor.color = lifeBarColor.Evaluate(life / lifeBar.maxValue);
    }

    private void Update() {

        if (Input.GetKeyDown(KeyCode.Escape)) {
            isPaused = !isPaused;
            Pause();
        }

        if (isPaused) {
            Time.timeScale = 0;
        } else {
            Time.timeScale = 1;
        }

        pontuacao.text = string.Format(Constantes.scoreStringFormat, LevelManager.instance.pontuacao);
    }


    public void BackMenu() {
        isPaused = false;
        Time.timeScale = 1;
        string[] sc = { "Menu" };
        GameManager.instance.LoadScenes(sc);
    }

    public void HabilidadeIMG(Sprite hab) {
        habilidadeSelecionada.sprite = hab;
    }

    private void Pause() {
        pausePanel.SetActive(isPaused);
        buttons.SetActive(isPaused);
        options.SetActive(false);
    }

    public void Despausar() {
        pausePanel.SetActive(false);
        buttons.SetActive(false);
        options.SetActive(false);
        isPaused = false;
    }

    public void ShowOptions() {
        buttons.SetActive(false);
        options.SetActive(true);
    }

    public void BackButtons() {
        buttons.SetActive(true);
        options.SetActive(false);
    }

    public void GameUIdisable() {
        gameUI.SetActive(false);
    }

    private void Victory() {
        Time.timeScale = 1;
        this.enabled = false;
        GameUIdisable();
    }

    private void Defeat() {
        Time.timeScale = 1;
        this.enabled = false;
        GameUIdisable();
    }



}
