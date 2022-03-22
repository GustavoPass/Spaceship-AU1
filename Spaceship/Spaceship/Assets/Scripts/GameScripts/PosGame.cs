using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public sealed class PosGame : MonoBehaviour, IEndGame{

    public static PosGame instance { get; private set; }

    [SerializeField] private GameObject panelEndGame;

    [SerializeField] private Slider sliderLevelUP;
    [SerializeField] private Slider sliderXPGanha;
    [SerializeField] private TextMeshProUGUI leveltxt;
    [SerializeField] private TextMeshProUGUI xpGanhatxt;
    [SerializeField] private TextMeshProUGUI xpGanhaXpNecessaria;
    [SerializeField] private TextMeshProUGUI endGameText;
    [SerializeField] private Button menuBtn;

    [SerializeField] private ParticleSystem particulaLevel;

    private bool enableAnim;

    private int levelAtual;
    private int xpAnterior;
    private int xpNecessaria;
    private int xpGanhada;
    private int xpRestante;
    private int levelsUpados;
    private int xpText;
    private int barraXPStep;

    private int xpAtual;

    private bool IsLevel30;

    private void Start() {

        if (PosGame.instance != null) Destroy(this);

        PosGame.instance = this;
        panelEndGame.SetActive(false);
        endGameText.gameObject.SetActive(false);

        LevelManager.instance.OnDefeat += Defeat;
        LevelManager.instance.OnVictory += Victory;
    }

    private void OnDestroy() {
        LevelManager.instance.OnDefeat -= Defeat;
        LevelManager.instance.OnVictory -= Victory;
    }

    public void Defeat() {
        endGameText.gameObject.SetActive(true);
        endGameText.text = "Defeat";
        StartCoroutine(ShowScreen(2f));
    }

    public void Victory() {
        endGameText.gameObject.SetActive(true);
        endGameText.text = "Victory";
        StartCoroutine(ShowScreen(3f));
    }

    private void EndGameScreen() {

        if (GameManager.instance == null) return;

        levelAtual = GameManager.instance.levelPlayer;
        xpAnterior = GameManager.instance.currentXpLevel;
        xpGanhada = LevelManager.instance.xpGanha;
        xpNecessaria = Constantes.XpProjection(levelAtual);

        sliderXPGanha.maxValue = xpGanhada;
        sliderXPGanha.value = xpGanhada;
        barraXPStep = xpGanhada - (xpNecessaria - xpAnterior);

        sliderLevelUP.maxValue = xpNecessaria;
        sliderLevelUP.value = xpAnterior;
        xpAtual = xpAnterior;

        enableAnim = false;
        menuBtn.interactable = false;

        AtualizaUI();
        panelEndGame.SetActive(true);

        CalculaLevelUP();

        IsLevel30 = levelAtual >= Constantes.levelMax;

        if (!IsLevel30)
            StartCoroutine(AnimationTime());
        else 
            menuBtn.interactable = true;
    }


    private void CalculaLevelUP (){
        xpRestante = xpGanhada + xpAnterior;
        levelsUpados = 0;

       while (true) {
            //Calcula levels upados e XP restante do level atual
            if (levelAtual >= Constantes.levelMax) break;

            xpNecessaria = Constantes.XpProjection(levelAtual);
            int sub = xpRestante - xpNecessaria;

            if (sub < 0) break;

            xpRestante = sub;
            levelAtual += 1;
            levelsUpados += 1;
        }

        GameManager.instance.EndLevelSave(levelsUpados, xpRestante);

    }

    private void AtualizaUI() {
        leveltxt.text = (levelAtual - levelsUpados).ToString();
        xpText = (int)sliderXPGanha.value;
        xpGanhatxt.text = xpText.ToString("XP: 0");
        xpGanhaXpNecessaria.text = string.Format("{0:0} / {1}", sliderLevelUP.value, xpNecessaria);
    }

    public void BackMenu() {
        string[] sc = { "Menu" };
        GameManager.instance.LoadScenes(sc);
    }


    private void Update() {
        if (!enableAnim) return;

        SlideAnim();
        AtualizaUI();
    }

    private void SlideAnim() {
        if (sliderXPGanha.value == 0) {
            menuBtn.interactable = true;
            enableAnim = false;
        }

        if (levelsUpados > 0) {
            sliderLevelUP.value = Mathf.MoveTowards(sliderLevelUP.value, sliderLevelUP.maxValue, Time.deltaTime * sliderLevelUP.maxValue);
        } else {
            sliderLevelUP.value = Mathf.MoveTowards(sliderLevelUP.value, xpRestante, Time.deltaTime * sliderLevelUP.maxValue);
        }

        sliderXPGanha.value = Mathf.MoveTowards(sliderXPGanha.value, barraXPStep, Time.deltaTime * sliderLevelUP.maxValue);

        if (sliderLevelUP.value == sliderLevelUP.maxValue) {
            enableAnim = false;
            StartCoroutine(LevelUpUI());
        }
    }

    private IEnumerator LevelUpUI() {
        //xpAnterior = 0;
        levelsUpados -= 1;
        
        particulaLevel.Play();

        yield return new WaitForSeconds(1.5f);
        xpNecessaria = Constantes.XpProjection(levelAtual - levelsUpados);
        sliderLevelUP.value = 0;
        sliderLevelUP.maxValue = xpNecessaria;
        enableAnim = true;
        barraXPStep = barraXPStep - (int)sliderLevelUP.maxValue;
        AtualizaUI();
    }

    private IEnumerator AnimationTime() {
        yield return new WaitForSeconds(2f);
        xpNecessaria = Constantes.XpProjection(levelAtual - levelsUpados);
        enableAnim = true;
    }

    private IEnumerator ShowScreen(float time) {
        yield return new WaitForSeconds(time);
        EndGameScreen();
        endGameText.gameObject.SetActive(false);
    }

}
