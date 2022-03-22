//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public sealed class GameSettings : MonoBehaviour{

    public Slider masterVolume;
    public Slider musicVolume;
    public Slider effectsVolume;

    public TMP_Dropdown drop;
    private Resolution[] resolutions;
    private int indexRes;

    public Button saveSettingsButton;

    void Start() {
        SetUIValues();
    }

    public void SetUIValues() {
        resolutions = Screen.resolutions;
        System.Array.Reverse(resolutions);
        
        masterVolume.value = GameManager.instance.masterVolume;
        musicVolume.value = GameManager.instance.musicVolume;
        effectsVolume.value = GameManager.instance.effectsVolume;

        indexRes = GameManager.instance.resolutionIndex;
        drop.value = indexRes;

        drop.options.Clear();
        for (int i = 0; i < resolutions.Length; i++) {
            drop.options.Add(new TMP_Dropdown.OptionData() { text = resolutions[i].width + " x " + resolutions[i].height });
        }

        saveSettingsButton.interactable = false;
    }

    private void OnEnable() {
        drop.value = GameManager.instance.resolutionIndex;

        masterVolume.value = GameManager.instance.masterVolume;
        musicVolume.value = GameManager.instance.musicVolume;
        effectsVolume.value = GameManager.instance.effectsVolume;

        saveSettingsButton.interactable = false;
    }

    public void MasterChanged(float sliderValue) {
        GameManager.instance.AudioMixerChangeSingleValue("MasterVolume", sliderValue);
        saveSettingsButton.interactable = true;
    }

    public void MusicChanged(float sliderValue) {
        GameManager.instance.AudioMixerChangeSingleValue("MusicVolume", sliderValue);
        saveSettingsButton.interactable = true;
    }

    public void EffectsChanged(float sliderValue) {
        GameManager.instance.AudioMixerChangeSingleValue("EffectsVolume", sliderValue);
        saveSettingsButton.interactable = true;
    }

    public void ResolutionChanged(int index) {
        if (resolutions == null) return;
        indexRes = index;
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, true);
        saveSettingsButton.interactable = true;
    }

    public void SaveSettings() {
        //SetPlayerPrefs
        PlayerPrefs.SetFloat(Constantes.masterVolumeString, masterVolume.value);
        PlayerPrefs.SetFloat(Constantes.musicVolumeString, musicVolume.value);
        PlayerPrefs.SetFloat(Constantes.effectsVolumeString, effectsVolume.value);

        PlayerPrefs.SetInt(Constantes.screenWidthString, resolutions[indexRes].width);
        PlayerPrefs.SetInt(Constantes.screenHeightString, resolutions[indexRes].height);
        PlayerPrefs.SetInt(Constantes.screenResolutionIndex, drop.value);

        GameManager.instance.AudioMixerSetValues();

        saveSettingsButton.interactable = false;
    }

    public void CancelChanges() {
        Screen.SetResolution(GameManager.instance.currentResolution.width, 
                                        GameManager.instance.currentResolution.height, 
                                        true);
        GameManager.instance.AudioMixerSetValues();
        saveSettingsButton.interactable = false;
    }


}
