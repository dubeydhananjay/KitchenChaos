using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePauseUI : MonoBehaviour {
    
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button optionsButton;
 
    private void Awake() {
        mainMenuButton.onClick.AddListener(() => {
            Loader.LoadTargetScene(Loader.Scene.MainMenuScene);
        });
        optionsButton.onClick.AddListener(() => {
            Hide();
            OptionsUI.Instance.Show(Show);
        });
    }
    private void Start() {
        GameManager.Instance.OnGamePaused += GameManager_OnGamePaused;
        Hide();
    }

    private void GameManager_OnGamePaused(object sender, GameManager.OnGamePausedEventArgs e) {
        if(e.isGamePaused) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        gameObject.SetActive(true);
        optionsButton.Select();
    }

    private void Hide() {
        gameObject.SetActive(false);
    }
}
