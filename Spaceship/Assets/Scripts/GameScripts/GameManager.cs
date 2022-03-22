using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using SaveLoadSystem;

public sealed class GameManager : MonoBehaviour{

    public static GameManager instance { get; private set; }
    [SerializeField] private AudioMixer mixer;
    public SpaceShipStatus status;

    public int levelPlayer;
    public int pointsToAddStatus;
    public int currentXpLevel;

    private Dictionary<string, int> HighScores;

    //variaveis de settings
    public float masterVolume { get; private set; }
    public float musicVolume { get; private set; }
    public float effectsVolume { get; private set; }
    public int resolutionIndex { get; private set; }
    public Resolution currentResolution;

    public string[] CurrentScenes;
    public string levelName;

    public enum Difficult {
        Easy = 1,
        Medium = 2,
        Hard = 3
    };
    public Difficult difficultLevel;

    void Awake() {
        if (instance != null) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }

        GetPlayerPrefs();

        LoadData();
        verificarIntegridade();
        DontDestroyOnLoad(this);
    }

    private void Start() {
        
        Screen.SetResolution(currentResolution.width, currentResolution.height, true);

        if (mixer == null) return;
        AudioMixerSetValues();

        CurrentScenes = new string[1];
        CurrentScenes[0] = "Menu";
    }

    public void verificarIntegridade() {
        if (levelPlayer <= 0 || levelPlayer > 30) {
            levelPlayer = 1;
        }

        if (currentXpLevel > Constantes.XpProjection(levelPlayer)) {
            currentXpLevel = 0;
        }
       
        if (status.totalPontosUsados + pointsToAddStatus > (levelPlayer - 1) * Constantes.pointsLevelUp) {
            pointsToAddStatus = (levelPlayer - 1) * Constantes.pointsLevelUp;
            status.healthPoints = 0;
            status.damagePoints = 0;
            status.reloadPoints = 0;
            status.velocityPoints = 0;
        }
    }

    private void GetPlayerPrefs() {
        currentResolution.height = PlayerPrefs.GetInt(Constantes.screenHeightString, Screen.currentResolution.height);
        currentResolution.width = PlayerPrefs.GetInt(Constantes.screenWidthString, Screen.currentResolution.width);
        resolutionIndex = PlayerPrefs.GetInt(Constantes.screenResolutionIndex, 0);

        masterVolume = PlayerPrefs.GetFloat(Constantes.masterVolumeString, 1);
        musicVolume = PlayerPrefs.GetFloat(Constantes.musicVolumeString, 1);
        effectsVolume = PlayerPrefs.GetFloat(Constantes.effectsVolumeString, 1);
    }


    #region DIFFICULT
    public string GetDifficultName(int i) {
        Difficult dl = (Difficult)i;
        return dl.ToString();
    }

    public int GetDifficultMultiply() {
        switch (difficultLevel) {
            case Difficult.Easy:
                return 1;
            case Difficult.Medium:
                return 15;
            case Difficult.Hard:
                return 30;
            default:
                return 1;
        }
        
    }

    public void SetDifficult(int i) {
        difficultLevel = (Difficult)i;
    }

    #endregion


    #region GET VARIAVEIS
    public int GetLevelHighScore(string levelName) {
        if (HighScores.ContainsKey(levelName)) return HighScores[levelName];
        else return 0;
    }

    public void ResetScore() {
        HighScores.Clear();
    }
    #endregion

    #region AUDIO MUSIC SOUND
    public void AudioMixerSetValues() {
        GetPlayerPrefs();

        mixer.SetFloat("MasterVolume", Mathf.Clamp(Mathf.Log10(masterVolume) * 20, -80, 0));
        mixer.SetFloat("MusicVolume", Mathf.Clamp(Mathf.Log10(musicVolume) * 20, -80, 0));
        mixer.SetFloat("EffectsVolume", Mathf.Clamp(Mathf.Log10(effectsVolume) * 20, -80, 0));
    }

    public void AudioMixerChangeSingleValue(string name, float value) {
        mixer.SetFloat(name, Mathf.Clamp(Mathf.Log10(value) * 20, -80, 0));
    }
    #endregion


    #region SAVE - LOAD DATA
    public void SaveStatus() {
        GameData save = SaveSystem.LoadData();
        save.healthPoints = status.healthPoints;
        save.damagePoints = status.damagePoints;
        save.reloadPoints = status.reloadPoints;
        save.velocityPoints = status.velocityPoints;
        save.statusPointsRemain = pointsToAddStatus;
        SaveSystem.SaveData(save);
    }

    private void LoadData() {
        GameData load = SaveSystem.LoadData();

        status.healthPoints = load.healthPoints;
        status.damagePoints = load.damagePoints;
        status.reloadPoints = load.reloadPoints;
        status.velocityPoints = load.velocityPoints;

        levelPlayer = load.levelPlayer;
        pointsToAddStatus = load.statusPointsRemain;
        currentXpLevel = load.currentXP;

        HighScores = new Dictionary<string, int>();
        HighScores = load.levelScore;
    }

    public void EndLevelSave(int levelUpados, int xpRemain) {
        GameData save = SaveSystem.LoadData();

        levelPlayer += levelUpados;
        pointsToAddStatus += Constantes.pointsLevelUp * levelUpados;
        currentXpLevel = xpRemain;

        if (save.levelScore.ContainsKey(levelName)) {
            int savedScore = save.levelScore[levelName];

            if (savedScore < LevelManager.instance.pontuacao) {
                //Verifica se a pontuação anterior é menor para sobrescrever
                save.levelScore[levelName] = LevelManager.instance.GetScore();
                HighScores[levelName] = LevelManager.instance.GetScore();
            }

        } else {
            save.levelScore.Add(levelName, LevelManager.instance.GetScore());
            HighScores.Add(levelName, LevelManager.instance.GetScore());
        }
        
        save.levelPlayer = levelPlayer;
        save.statusPointsRemain = pointsToAddStatus;
        save.currentXP = currentXpLevel;
        SaveSystem.SaveData(save);
    }
    #endregion


    #region LOAD SCENES
    public void RemoveLoadingScene() {
        SceneManager.UnloadSceneAsync(Constantes.sceneLoad);
    }

    public void LoadScenes(string[] scenesToLoadName) {
        StartCoroutine(UnloadAndLoadScenesAsync(scenesToLoadName));
    }

    private IEnumerator UnloadAndLoadScenesAsync(string[] scenesToLoadName) {

        AsyncOperation loadingScene = SceneManager.LoadSceneAsync(Constantes.sceneLoad, LoadSceneMode.Additive);

        //Carrega a cena se loading
        while (!loadingScene.isDone) {
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        //Descarrega as cenas atuais
        for (int i = 0; i < CurrentScenes.Length; i++) {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(CurrentScenes[i]);

            while (!asyncUnload.isDone) {
                yield return null;
            }
        }

        //Carrega as próximas cenas
        for (int i = 0; i < scenesToLoadName.Length; i++) {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenesToLoadName[i], LoadSceneMode.Additive);

            while (!asyncLoad.isDone) {
                yield return null;
            }
        }

        CurrentScenes = new string[scenesToLoadName.Length];
        CurrentScenes = scenesToLoadName;

        //Começa a carregar objetos para pooling
        LoadManager.instance.StartLoadObjects();
    }
    #endregion
}