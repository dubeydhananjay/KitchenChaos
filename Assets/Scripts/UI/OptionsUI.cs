using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour {

    public static OptionsUI Instance { get; private set; }
    [SerializeField] private Transform pressToKeyBindTransform;
    [SerializeField] private Button closeButton;
    [SerializeField] private Slider soundEffectVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    private Action CloseButtonAction;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        closeButton.onClick.AddListener(() => {
            CloseButtonAction?.Invoke();
            Hide();
        });

        soundEffectVolumeSlider.value = SoundManager.Instance.Volume;
        musicVolumeSlider.value = MusicManager.Instance.Volume;

        soundEffectVolumeSlider.onValueChanged.AddListener((val) =>
         SoundManager.Instance.ChangeVolume(soundEffectVolumeSlider.value));
        musicVolumeSlider.onValueChanged.AddListener((val) =>
         MusicManager.Instance.ChangeVolume(musicVolumeSlider.value));
        Hide();
    }

    public void Show(Action CloseButtonAction) {
        this.CloseButtonAction = CloseButtonAction;
        gameObject.SetActive(true);
        soundEffectVolumeSlider.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }

    public void ShowPressToKeyBindTransform() {
        pressToKeyBindTransform.gameObject.SetActive(true);
    }

    public void HidePressToKeyBindTransform() {
        pressToKeyBindTransform.gameObject.SetActive(false);
    }
    
}
